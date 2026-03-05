using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.Feedback;
using OldVikings.Api.Helper;
using OldVikings.Api.Interfaces;
using OldVikings.Api.Services;

namespace OldVikings.Api.Repositories;

public class FeedbackRepository(OldVikingsContext dbContext, IMapper mapper, DiscordFeedbackSender discordFeedbackSender, ILogger<FeedbackRepository> logger) : IFeedbackRepository
{
    public async Task VoteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feedback = await dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (feedback is null)
        {
            throw new KeyNotFoundException($"Feedback with ID {id} not found.");
        }
        feedback.VotesCount++;
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            await discordFeedbackSender.UpdateFeedbackMessageAsync(feedback, cancellationToken);

            if (feedback.DiscordThreadId is not null)
            {
                await discordFeedbackSender.PostThreadMessageAsync(
                    feedback.DiscordThreadId.Value,
                    $"⭐ New vote for this feedback. Total votes: {feedback.VotesCount}",
                    cancellationToken
                );
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to update Discord message for feedback {Id} after voting", id);
        }
    }

    public async Task<FeedbackDto?> GetFeedbackAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feedback = await dbContext.Feedbacks
            .ProjectTo<FeedbackDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

        return feedback;
    }

    public async Task<List<FeedbackDto>> GetFeedbacksByStatusAsync(int[] status, CancellationToken cancellationToken = default)
    {
        IQueryable<FeedbackItem> query = dbContext.Feedbacks;
        if (status is { Length: > 0})
        {
            query = query.Where(x => status.Contains((int)x.Status));
        }
        var feedbacks = await query
            .Where(x => x.Visibility == FeedbackVisibility.Public)
            .ProjectTo<FeedbackDto>(mapper.ConfigurationProvider)
            .OrderByDescending(x => x.CreatedAtUtc)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return feedbacks;
    }

    public async Task RetryDiscordPostAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var feedback = await dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
        if (feedback is null)
        {
            throw new KeyNotFoundException($"Feedback with ID {id} not found.");
        }

        try
        {
            feedback.DiscordAttempts++;
            feedback.DiscordLastAttemptAtUtc = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);

            var (ch, msg) = await discordFeedbackSender.SendFeedbackAsync(feedback, cancellationToken);
            feedback.DiscordChannelId = ch;
            feedback.DiscordMessageId = msg;

            var threadId = await discordFeedbackSender.CreateThreadAsync(ch, msg, feedback.Title ?? "Unknown Title", cancellationToken);
            feedback.DiscordThreadId = threadId;

            feedback.DiscordPosted = true;
            await dbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception e)
        {
            logger.LogError(e, "Retry Discord failed for {Id}", id);
            feedback.DiscordPosted = false;
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<FeedbackDto> CreateFeedbackAsync(CreateFeedbackDto dto, CancellationToken cancellationToken = default)
    {
        var newFeedback = mapper.Map<FeedbackItem>(dto);

        try
        {
            await dbContext.Feedbacks.AddAsync(newFeedback, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Best-effort Discord posting (do not block response)

                try
                {
                    newFeedback.DiscordAttempts++;
                    newFeedback.DiscordLastAttemptAtUtc = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(CancellationToken.None);

                    var (ch, msg) = await discordFeedbackSender.SendFeedbackAsync(newFeedback, CancellationToken.None);
                    newFeedback.DiscordChannelId = ch;
                    newFeedback.DiscordMessageId = msg;

                    var threadId = await discordFeedbackSender.CreateThreadAsync(ch, msg, newFeedback.Title ?? "Unknown Title", CancellationToken.None);
                    newFeedback.DiscordThreadId = threadId;

                    newFeedback.DiscordPosted = true;
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Discord post/thread failed for Feedback {Id}", newFeedback.Id);
                    newFeedback.DiscordPosted = false;
                    await dbContext.SaveChangesAsync(CancellationToken.None);
                }

            return mapper.Map<FeedbackDto>(newFeedback);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating feedback");
            throw;
        }
    }

    public async Task<FeedbackDto> UpdateFeedbackAsync(UpdateFeedbackDto dto, CancellationToken cancellationToken = default)
    {
        var feedbackToUpdate = await dbContext.Feedbacks.FirstOrDefaultAsync(f => f.Id == dto.Id, cancellationToken);

        if (feedbackToUpdate is null)
        {
            throw new KeyNotFoundException($"Feedback with ID {dto.Id} not found.");
        }

        mapper.Map(dto, feedbackToUpdate);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<FeedbackDto>(feedbackToUpdate);
    }
}