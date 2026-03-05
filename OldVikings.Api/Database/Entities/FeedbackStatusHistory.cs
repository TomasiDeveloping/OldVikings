using OldVikings.Api.Helper;

namespace OldVikings.Api.Database.Entities;

public class FeedbackStatusHistory 
{
        public Guid Id { get; set; }

        public Guid FeedbackItemId { get; set; }
        public FeedbackItem FeedbackItem { get; set; } = null!;

        public FeedbackStatus OldStatus { get; set; }
        public FeedbackStatus NewStatus { get; set; }

        public string? Note { get; set; }

        public DateTime ChangedAtUtc { get; set; }

        public ulong DiscordUserId { get; set; }
        public string DiscordUserName { get; set; } = null!;
}