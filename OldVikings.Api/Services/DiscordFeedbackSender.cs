using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using OldVikings.Api.Classes;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Helper;

namespace OldVikings.Api.Services;

public sealed class DiscordFeedbackSender
{
    // HttpClient used to communicate with the Discord REST API
    private readonly HttpClient _http;

    // Target Discord channel where feedback messages will be posted
    private readonly ulong _channelId;

    // Configuration options for the Discord bot (token, base URL, etc.)
    private readonly DiscordBotOptions _discordBotOptions;

    public DiscordFeedbackSender(HttpClient http, IOptions<DiscordBotOptions> options)
    {
        _discordBotOptions = options.Value;
        _http = http;

        // Convert configured channel ID from string to ulong
        _channelId = ulong.Parse(_discordBotOptions.FeedbackChannelId);

        // Add bot authentication header required by Discord API
        var token = _discordBotOptions.Token;
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bot", token);
    }

    /// <summary>
    /// Builds the Discord embed payload representing the feedback item.
    /// Embeds are rich messages with structured fields.
    /// </summary>
    private static object BuildEmbed(FeedbackItem item)
    {
        // Discord embed description has a character limit,
        // so we truncate the message if it is too long
        var desc = item.Message.Length > 3500
            ? item.Message[..3500] + "…"
            : item.Message;

        return new
        {
            title = item.Title ?? "New Feedback",
            description = desc,
            fields = new object[]
            {
                new { name = "Id", value = item.Id.ToString(), inline = false },
                new { name = "Category", value = item.Category.ToString(), inline = true },
                new { name = "Visibility", value = item.Visibility.ToString(), inline = true },
                new { name = "Status", value = item.Status.ToString(), inline = true },
                new { name = "Submitted by", value = item.IsAnonymous ? "Anonymous" : (item.DisplayName ?? "Unknown"), inline = true },
                new { name = "Votes", value = item.VotesCount.ToString(), inline = true },
                new { name = "Created (UTC)", value = item.CreatedAtUtc.ToString("dd.MM.yyyy HH:mm:ss"), inline = false },
            }
        };
    }

    /// <summary>
    /// Builds the interactive buttons shown below the feedback message.
    /// These buttons allow moderators to update the feedback status.
    /// </summary>
    public static object[] BuildButtons(Guid feedbackId, FeedbackStatus status)
    {
        // If feedback is already finalized, do not show any buttons
        if (status is FeedbackStatus.Implemented or FeedbackStatus.Rejected or FeedbackStatus.Archived)
        {
            return [];
        }

        var row = new List<object>
        {
            // Button to mark feedback as "Under Review"
            Btn(feedbackId, "UnderReview", "🟡 Under Review", 2,
                disabled: status == FeedbackStatus.UnderReview)
        };

        // Show "Planned" button unless it is already planned
        if (status != FeedbackStatus.Planned)
        {
            row.Add(Btn(feedbackId, "Planned", "🗓️ Planned", 1));
        }

        // Mark as implemented
        row.Add(Btn(feedbackId, "Implemented", "✅ Implemented", 3));

        // Reject option (with reason dialog handled elsewhere)
        if (status is FeedbackStatus.New or FeedbackStatus.UnderReview or FeedbackStatus.Planned)
        {
            row.Add(RejectBtn(feedbackId));
        }

        // Archive button
        row.Add(Btn(feedbackId, "Archived", "📦 Archive", 2));

        // Discord requires buttons to be inside an action row
        return row.Count == 0
            ? []
            :
            [
                new
                {
                    type = 1,
                    components = row.ToArray()
                }
            ];
    }

    /// <summary>
    /// Helper to build a standard status button.
    /// </summary>
    private static object Btn(Guid id, string status, string label, int style, bool disabled = false) => new
    {
        type = 2,
        style,
        label,
        // Used later to identify the interaction
        custom_id = $"fb:{id}:set:{status}",
        disabled
    };

    /// <summary>
    /// Special button for rejecting feedback with a reason.
    /// </summary>
    private static object RejectBtn(Guid id) => new
    {
        type = 2,
        style = 4,
        label = "❌ Reject (reason…)",
        custom_id = $"fb:{id}:reject",
        disabled = false
    };

    // Minimal DTO used to deserialize Discord message response
    private sealed class DiscordMessage { public string Id { get; init; } = null!; }

    // Minimal DTO used to deserialize Discord thread response
    private sealed class DiscordThread { public string Id { get; init; } = null!; }

    /// <summary>
    /// Sends a new feedback message to the configured Discord channel.
    /// Returns the channel ID and message ID.
    /// </summary>
    public async Task<(ulong channelId, ulong messageId)> SendFeedbackAsync(FeedbackItem item, CancellationToken ct)
    {
        var payload = new
        {
            content = (string?)null,
            embeds = new[] { BuildEmbed(item) },
            components = BuildButtons(item.Id, item.Status)
        };

        var url = $"{_discordBotOptions.DiscordChannelBaseUrl}/{_channelId}/messages";
        using var res = await _http.PostAsJsonAsync(url, payload, ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Discord send failed: {(int)res.StatusCode}\n{await res.Content.ReadAsStringAsync(ct)}");

        var msg = await res.Content.ReadFromJsonAsync<DiscordMessage>(cancellationToken: ct)
                  ?? throw new InvalidOperationException("Discord message response empty.");

        return (_channelId, ulong.Parse(msg.Id));
    }

    /// <summary>
    /// Updates an existing Discord feedback message (embed + buttons).
    /// </summary>
    public async Task UpdateFeedbackMessageAsync(FeedbackItem item, CancellationToken ct)
    {
        // If the feedback was never posted to Discord, skip update
        if (item.DiscordChannelId is null || item.DiscordMessageId is null)
            return;

        var payload = new
        {
            embeds = new[] { BuildEmbed(item) },
            components = BuildButtons(item.Id, item.Status)
        };

        var url = $"{_discordBotOptions.DiscordChannelBaseUrl}/{item.DiscordChannelId}/messages/{item.DiscordMessageId}";

        using var req = new HttpRequestMessage(HttpMethod.Patch, url);
        req.Content = JsonContent.Create(payload);

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException(
                $"Discord update failed: {(int)res.StatusCode}\n{await res.Content.ReadAsStringAsync(ct)}");
    }

    /// <summary>
    /// Creates a discussion thread attached to a feedback message.
    /// </summary>
    public async Task<ulong> CreateThreadAsync(ulong channelId, ulong messageId, string feedbackTitle, CancellationToken ct)
    {
        var payload = new
        {
            name = $"Feedback {feedbackTitle}",
            auto_archive_duration = 10080 // 7 days 
        };

        var url = $"{_discordBotOptions.DiscordChannelBaseUrl}/{channelId}/messages/{messageId}/threads";
        using var res = await _http.PostAsJsonAsync(url, payload, ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Create thread failed: {(int)res.StatusCode}\n{await res.Content.ReadAsStringAsync(ct)}");

        var t = await res.Content.ReadFromJsonAsync<DiscordThread>(cancellationToken: ct)
                ?? throw new InvalidOperationException("Thread response empty.");

        return ulong.Parse(t.Id);
    }

    /// <summary>
    /// Changes the thread auto archive duration to 24 hours.
    /// </summary>
    public async Task SetThreadAutoArchive24HAsync(ulong threadId, CancellationToken ct)
    {
        var url = $"{_discordBotOptions.DiscordChannelBaseUrl}/{threadId}";
        var req = new HttpRequestMessage(new HttpMethod("PATCH"), url)
        {
            Content = JsonContent.Create(new { auto_archive_duration = 1440 }) // 24h
        };

        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Patch thread failed: {(int)res.StatusCode}\n{await res.Content.ReadAsStringAsync(ct)}");
    }

    /// <summary>
    /// Posts a text message inside a Discord thread.
    /// </summary>
    public async Task PostThreadMessageAsync(ulong threadId, string text, CancellationToken ct)
    {
        var url = $"{_discordBotOptions.DiscordChannelBaseUrl}/{threadId}/messages";
        using var res = await _http.PostAsJsonAsync(url, new { content = text }, ct);
        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Post thread msg failed: {(int)res.StatusCode}\n{await res.Content.ReadAsStringAsync(ct)}");
    }
}