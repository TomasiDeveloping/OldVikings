using OldVikings.Api.Helper;

namespace OldVikings.Api.DataTransferObjects.Feedback;

public class CreateFeedbackDto
{
    public FeedbackVisibility Visibility { get; set; }

    public FeedbackCategory Category { get; set; }

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? DisplayName { get; set; }
}