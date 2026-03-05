using OldVikings.Api.Helper;

namespace OldVikings.Api.DataTransferObjects.Feedback;

public class UpdateFeedbackDto
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

    public int VotesCount { get; set; }
}