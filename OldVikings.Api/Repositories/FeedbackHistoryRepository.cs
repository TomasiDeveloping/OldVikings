using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.DataTransferObjects.FeedbackHistory;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class FeedbackHistoryRepository(OldVikingsContext dbContext, IMapper mapper) : IFeedbackHistoryRepository
{
    public async Task<List<FeedbackHistoryDto>> GetLast3Async(Guid feedbackId, CancellationToken cancellationToken = default)
    {
        var last3Histories = await dbContext.FeedbackStatusHistories
            .Where(x => x.FeedbackItemId == feedbackId)
            .OrderByDescending(x => x.ChangedAtUtc)
            .ProjectTo<FeedbackHistoryDto>(mapper.ConfigurationProvider)
            .Take(3)
            .ToListAsync(cancellationToken);

        return last3Histories;
    }
}