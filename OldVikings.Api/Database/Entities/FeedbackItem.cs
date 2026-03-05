using OldVikings.Api.Helper;

namespace OldVikings.Api.Database.Entities;

public class FeedbackItem
{
    public Guid Id { get; set; }

    public FeedbackVisibility Visibility { get; set; }

    public FeedbackCategory Category { get; set; }

    public string? Title { get; set; }

    public string Message { get; set; } = null!;

    public bool IsAnonymous { get; set; }

    public string? DisplayName { get; set; }

    public FeedbackStatus Status { get; set; }

    public string? StatusMessage { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public int VotesCount { get; set; }

    // Discord refs
    public ulong? DiscordChannelId { get; set; }
    public ulong? DiscordMessageId { get; set; }
    public ulong? DiscordThreadId { get; set; }

    // who updated
    public ulong? UpdatedByDiscordUserId { get; set; }
    public string? UpdatedByDiscordName { get; set; }

    // best-effort flags
    public bool DiscordPosted { get; set; }
    public int DiscordAttempts { get; set; }
    public DateTime? DiscordLastAttemptAtUtc { get; set; }
}