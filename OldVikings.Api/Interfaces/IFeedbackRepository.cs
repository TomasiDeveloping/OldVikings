using OldVikings.Api.DataTransferObjects.Feedback;

namespace OldVikings.Api.Interfaces;

public interface IFeedbackRepository
{
    Task VoteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<FeedbackDto?> GetFeedbackAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<FeedbackDto>> GetFeedbacksByStatusAsync(int[] status, CancellationToken cancellationToken = default);

    Task RetryDiscordPostAsync(Guid id, CancellationToken cancellationToken = default);

    Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, CancellationToken cancellationToken = default);

    Task<FeedbackDto> UpdateFeedbackAsync(UpdateFeedbackDto dto, CancellationToken cancellationToken = default);
}