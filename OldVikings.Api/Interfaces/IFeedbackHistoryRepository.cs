using OldVikings.Api.DataTransferObjects.FeedbackHistory;

namespace OldVikings.Api.Interfaces;

public interface IFeedbackHistoryRepository
{
    Task<List<FeedbackHistoryDto>> GetLast3Async(Guid feedbackId, CancellationToken cancellationToken = default);
}