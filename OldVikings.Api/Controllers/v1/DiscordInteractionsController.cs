using Microsoft.AspNetCore.Mvc;
using NSec.Cryptography;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Helper;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OldVikings.Api.Classes;
using OldVikings.Api.Services;

namespace OldVikings.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class DiscordInteractionsController(
    OldVikingsContext db, 
    IOptions<DiscordBotOptions> options, 
    DiscordFeedbackSender discordFeedbackSender, 
    ILogger<DiscordInteractionsController> logger) : ControllerBase
{
    private readonly DiscordBotOptions _discordBotOptions = options.Value;

    /// <summary>
    /// Main endpoint that receives Discord interaction events
    /// (button clicks, modal submissions, ping validation).
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Handle(CancellationToken ct)
    {
        // Discord signs every interaction request using Ed25519.
        // We must verify the request before processing it.
        var sigHex = Request.Headers["X-Signature-Ed25519"].ToString();
        var ts = Request.Headers["X-Signature-Timestamp"].ToString();
        if (string.IsNullOrWhiteSpace(sigHex) || string.IsNullOrWhiteSpace(ts))
            return Unauthorized();

        // Read raw body because signature verification requires
        // the exact timestamp + raw payload bytes.
        using var ms = new MemoryStream();
        await Request.Body.CopyToAsync(ms, ct);
        var body = ms.ToArray();

        var publicKeyHex = _discordBotOptions.PublicKey;
        if (string.IsNullOrWhiteSpace(publicKeyHex))
            return Problem("Discord public key missing.");

        // Verify Discord signature
        if (!VerifyDiscordSignature(publicKeyHex, ts, body, sigHex))
            return Unauthorized();

        using var doc = JsonDocument.Parse(body);
        var root = doc.RootElement;

        // Interaction type (Discord docs)
        var type = root.GetProperty("type").GetInt32();

        // Type 1 = Ping (Discord verifying endpoint)
        if (type == 1)
            return new JsonResult(new { type = 1 });

        // Extract user info who triggered the interaction
        var (actorId, actorName) = ExtractUser(root);

        // Type 3 = Component interaction (button clicks)
        if (type == 3)
        {
            var customId = root.GetProperty("data").GetProperty("custom_id").GetString() ?? "";

            // Reject button opens a modal for entering a reason
            if (customId.EndsWith(":reject", StringComparison.OrdinalIgnoreCase))
            {
                var fbId = ParseFeedbackId(customId);
                return new JsonResult(new
                {
                    type = 9, // MODAL
                    data = new
                    {
                        custom_id = $"fb:{fbId}:reject:reason",
                        title = "Reject feedback",
                        components = new object[]
                        {
                            new {
                                type = 1,
                                components = new object[]
                                {
                                    new {
                                        type = 4,
                                        custom_id = "reason",
                                        style = 2, // paragraph
                                        label = "Reason",
                                        placeholder = "Short reason (e.g. duplicate, missing details, not reproducible...)",
                                        required = true,
                                        max_length = 300
                                    }
                                }
                            }
                        }
                    }
                });
            }

            // Parse status change from button custom id
            var (fbId2, newStatus) = ParseSetStatus(customId);

            // Reject must be handled via modal
            if (newStatus == FeedbackStatus.Rejected) return Ephemeral("Use the Reject button to provide a reason.");

            var item = await db.Feedbacks.FirstOrDefaultAsync(x => x.Id == fbId2, ct);

            if (item is null) return Ephemeral("Feedback not found.");

            var oldStatus = item.Status;

            // Update feedback status
            item.Status = newStatus;
            item.StatusMessage = null;
            item.UpdatedAtUtc = DateTime.UtcNow;
            item.UpdatedByDiscordUserId = actorId;
            item.UpdatedByDiscordName = actorName;

            // Save status change history
            db.FeedbackStatusHistories.Add(new FeedbackStatusHistory
            {
                FeedbackItemId = item.Id,
                OldStatus = oldStatus,
                NewStatus = item.Status,
                Note = null,
                ChangedAtUtc = item.UpdatedAtUtc.Value,
                DiscordUserId = actorId,
                DiscordUserName = actorName
            });

            await db.SaveChangesAsync(ct);

            // Load last 3 history entries for display
            var last3 = await LoadLast3History(item.Id, ct);
            var historyText = BuildHistoryText(last3);
            var updateText = BuildUpdateText(item.Status, actorName);

            // If feedback is finalized, update thread policy
            await ApplyFinalThreadPolicyIfNeeded(item, ct);

            // Type 7 = Update original message
            return new JsonResult(new
            {
                type = 7,
                data = BuildUpdatedMessage(item, updateText, historyText)
            });
        }

        // Type 5 = Modal submission
        if (type != 5) return Ephemeral("Unsupported interaction type.");
        {
            var data = root.GetProperty("data");
            var modalId = data.GetProperty("custom_id").GetString() ?? "";

            if (!modalId.Contains(":reject:reason", StringComparison.OrdinalIgnoreCase))
                return Ephemeral("Unsupported modal.");

            var fbId = ParseFeedbackId(modalId);

            // Extract rejection reason
            var reason = ExtractModalValue(data, "reason")?.Trim();

            if (string.IsNullOrWhiteSpace(reason))
                return Ephemeral("Reason is required.");

            var item = await db.Feedbacks.FirstOrDefaultAsync(x => x.Id == fbId, ct);
            if (item is null)
                return Ephemeral("Feedback not found.");

            var oldStatus = item.Status;

            // Apply rejection
            item.Status = FeedbackStatus.Rejected;
            item.StatusMessage = reason;
            item.UpdatedAtUtc = DateTime.UtcNow;
            item.UpdatedByDiscordUserId = actorId;
            item.UpdatedByDiscordName = actorName;

            // Save history entry
            db.FeedbackStatusHistories.Add(new FeedbackStatusHistory
            {
                FeedbackItemId = item.Id,
                OldStatus = oldStatus,
                NewStatus = item.Status,
                Note = reason,
                ChangedAtUtc = item.UpdatedAtUtc.Value,
                DiscordUserId = actorId,
                DiscordUserName = actorName
            });

            await db.SaveChangesAsync(ct);

            var last3 = await LoadLast3History(item.Id, ct);
            var historyText = BuildHistoryText(last3);
            var updateText = BuildUpdateText(item.Status, actorName);

            await ApplyFinalThreadPolicyIfNeeded(item, ct);

            return new JsonResult(new
            {
                type = 7,
                data = BuildUpdatedMessage(item, updateText, historyText)
            });
        }

    }

    /// <summary>
    /// Sends an ephemeral message (only visible to the user).
    /// </summary>
    public static IActionResult Ephemeral(string content) =>
        new JsonResult(new { type = 4, data = new { content, flags = 64 } });

    /// <summary>
    /// Determines if a feedback status is final (no more changes expected).
    /// </summary>
    private static bool IsFinal(FeedbackStatus s)
        => s is FeedbackStatus.Implemented or FeedbackStatus.Rejected or FeedbackStatus.Archived;

    /// <summary>
    /// If feedback is finalized, apply thread policy (short archive + info message).
    /// </summary>
    private async Task ApplyFinalThreadPolicyIfNeeded(FeedbackItem item, CancellationToken ct)
    {
        if (!IsFinal(item.Status) || !item.DiscordThreadId.HasValue)
            return;

        try
        {
            await discordFeedbackSender.SetThreadAutoArchive24HAsync(item.DiscordThreadId.Value, ct);
            await discordFeedbackSender.PostThreadMessageAsync(
                item.DiscordThreadId.Value,
                "This feedback is closed. The thread will auto-archive after 24 hours of inactivity.",
                ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Thread final policy failed for Feedback {Id}", item.Id);
        }
    }

    /// <summary>
    /// Builds a human-readable status update message.
    /// </summary>
    private static string BuildUpdateText(FeedbackStatus status, string actorName) =>
        status switch
        {
            FeedbackStatus.UnderReview => $"Status set to **Under Review** by **{actorName}**.",
            FeedbackStatus.Planned => $"Status set to **Planned** by **{actorName}**.",
            FeedbackStatus.Implemented => $"Marked as **Implemented** by **{actorName}**.",
            FeedbackStatus.Rejected => $"Rejected by **{actorName}**.",
            FeedbackStatus.Archived => $"Archived by **{actorName}**.",
            _ => $"Status updated by **{actorName}**."
        };

    /// <summary>
    /// Loads the latest 3 status history entries.
    /// </summary>
    private async Task<List<FeedbackStatusHistory>> LoadLast3History(Guid feedbackId, CancellationToken ct)
    {
        var last3 = await db.FeedbackStatusHistories
            .Where(x => x.FeedbackItemId == feedbackId)
            .OrderByDescending(x => x.ChangedAtUtc)
            .Take(3)
            .ToListAsync(ct);

        last3.Reverse();
        return last3;
    }

    /// <summary>
    /// Converts history entries to readable text.
    /// </summary>
    private static string BuildHistoryText(List<FeedbackStatusHistory> entries)
    {
        if (entries.Count == 0) return "—";

        var sb = new StringBuilder();
        foreach (var e in entries)
        {
            sb.Append(e.ChangedAtUtc.ToString("u").Replace(" UTC", "Z"));
            sb.Append(" — ");
            sb.Append(e.NewStatus switch
            {
                FeedbackStatus.UnderReview => "Under Review",
                FeedbackStatus.Planned => "Planned",
                FeedbackStatus.Implemented => "Implemented",
                FeedbackStatus.Rejected => "Rejected",
                FeedbackStatus.Archived => "Archived",
                _ => e.NewStatus.ToString()
            });
            sb.Append(" — by ");
            sb.Append(e.DiscordUserName);

            if (e.NewStatus == FeedbackStatus.Rejected && !string.IsNullOrWhiteSpace(e.Note))
            {
                sb.Append(" — Reason: ");
                sb.Append(e.Note);
            }
            sb.AppendLine();
        }
        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Builds the updated Discord embed message.
    /// </summary>
    private static object BuildUpdatedMessage(FeedbackItem item, string updateText, string historyText)
    {
        var fields = new List<object>
        {
            new { name = "Id", value = item.Id.ToString(), inline = false },
            new { name = "Category", value = item.Category.ToString(), inline = true },
            new { name = "Visibility", value = item.Visibility.ToString(), inline = true },
            new { name = "Status", value = item.Status.ToString(), inline = true },
            new { name = "Submitted by", value = item.IsAnonymous ? "Anonymous" : (item.DisplayName ?? "Unknown"), inline = true },
            new { name = "Votes", value = item.VotesCount.ToString(), inline = true },
            new { name = "Created (UTC)", value = item.CreatedAtUtc.ToString("dd.MM.yyyy HH:mm:ss"), inline = false },
            new { name = "Update", value = updateText, inline = false },
            new { name = "History (last 3)", value = historyText, inline = false },
        };

        if (!string.IsNullOrWhiteSpace(item.UpdatedByDiscordName) && item.UpdatedAtUtc.HasValue)
        {
            fields.Add(new { name = "Last updated by", value = item.UpdatedByDiscordName!, inline = true });
            fields.Add(new { name = "Last updated (UTC)", value = item.UpdatedAtUtc.Value.ToString("dd.MM.yyyy HH:mm:ss"), inline = true });
        }

        if (item.Status == FeedbackStatus.Rejected && !string.IsNullOrWhiteSpace(item.StatusMessage))
            fields.Add(new { name = "Reason", value = item.StatusMessage!, inline = false });

        var components = IsFinal(item.Status)
            ? []
            : DiscordFeedbackSender.BuildButtons(item.Id, item.Status);

        return new
        {
            content = (string?)null,
            embeds = new[]
            {
                new
                {
                    title = item.Title ?? "Feedback",
                    description = item.Message.Length > 3500 ? item.Message[..3500] + "…" : item.Message,
                    fields = fields.ToArray()
                }
            },
            components
        };
    }

    /// <summary>
    /// Verifies Discord request signature using Ed25519.
    /// </summary>
    private static bool VerifyDiscordSignature(string publicKeyHex, string timestamp, byte[] body, string signatureHex)
    {
        var publicKeyBytes = Convert.FromHexString(publicKeyHex);
        var signatureBytes = Convert.FromHexString(signatureHex);

        var algo = SignatureAlgorithm.Ed25519;
        var publicKey = PublicKey.Import(algo, publicKeyBytes, KeyBlobFormat.RawPublicKey);

        var tsBytes = Encoding.UTF8.GetBytes(timestamp);
        var data = new byte[tsBytes.Length + body.Length];
        Buffer.BlockCopy(tsBytes, 0, data, 0, tsBytes.Length);
        Buffer.BlockCopy(body, 0, data, tsBytes.Length, body.Length);

        return algo.Verify(publicKey, data, signatureBytes);
    }

    /// <summary>
    /// Extracts the Discord user who triggered the interaction.
    /// </summary>
    private static (ulong userId, string userName) ExtractUser(JsonElement root)
    {
        if (!root.TryGetProperty("member", out var member) || !member.TryGetProperty("user", out var user))
            return (0, "Unknown");

        var id = ulong.Parse(user.GetProperty("id").GetString()!);

        var name =
            (user.TryGetProperty("global_name", out var gn) && gn.ValueKind == JsonValueKind.String)
                ? gn.GetString()!
                : user.GetProperty("username").GetString()!;

        if (member.TryGetProperty("nick", out var nick) && nick.ValueKind == JsonValueKind.String)
            name = nick.GetString()!;

        return (id, name);

    }

    /// <summary>
    /// Extracts the feedback ID from a Discord custom_id.
    /// </summary>
    private static Guid ParseFeedbackId(string customId)
    {
        var parts = customId.Split(':', StringSplitOptions.RemoveEmptyEntries);
        return Guid.Parse(parts[1]);
    }

    /// <summary>
    /// Parses status change buttons.
    /// </summary>
    private static (Guid feedbackId, FeedbackStatus status) ParseSetStatus(string customId)
    {
        var parts = customId.Split(':', StringSplitOptions.RemoveEmptyEntries);
        var fbId = Guid.Parse(parts[1]);

        if (!Enum.TryParse(parts[3], true, out FeedbackStatus status))
            throw new InvalidOperationException("Unknown status.");

        return (fbId, status);
    }

    /// <summary>
    /// Extracts values from modal input components.
    /// </summary>
    private static string? ExtractModalValue(JsonElement modalData, string inputCustomId)
    {
        return (from row in modalData.GetProperty("components").EnumerateArray()
            from comp in row.GetProperty("components").EnumerateArray() 
            where comp.GetProperty("custom_id").GetString() == inputCustomId 
            select comp.GetProperty("value").GetString()).FirstOrDefault();
    }
}