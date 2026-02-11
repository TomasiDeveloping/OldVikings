using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Services;

public class WeeklyPlanService(
    OldVikingsContext dbContext,
    IPlayerRepository playerRepository,
    IPoolRepository poolRepository,
    IScheduleRepository scheduleRepository)
{
    public async Task<WeeklySchedule> GenerateWeekAsync(DateOnly weekStartMonday, CancellationToken cancellationToken)
    {
        // Safety check: the schedule must always start on a Monday
        if (weekStartMonday.DayOfWeek != DayOfWeek.Monday)
            throw new ArgumentException("The provided date must be a Monday.", nameof(weekStartMonday));

        // If the schedule for this week already exists, return it (idempotent job)
        var existing = await scheduleRepository.GetWeeklyAsync(weekStartMonday, cancellationToken);
        if (existing is not null) return existing;

        // At least two active (registered + approved) players are required
        // Otherwise Leader != VIP cannot be guaranteed
        var activeCount = await playerRepository.CountActiveAsync(cancellationToken);
        if (activeCount < 2) throw new ArgumentException("At least 2 active players required.");

        // Ensure all registered players exist in both pools (Leader + VIP)
        await poolRepository.SyncPoolsForActivePlayersAsync(cancellationToken);

        // Create the schedule within a transaction so the week is either fully created or not at all
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        // Double-check inside the transaction to avoid race conditions
        if (await scheduleRepository.WeekExistsAsync(weekStartMonday, cancellationToken))
        {
            await transaction.CommitAsync(cancellationToken);
            return (await scheduleRepository.GetWeeklyAsync(weekStartMonday, cancellationToken))!;
        }

        var schedule = new WeeklySchedule
        {
            Id = Guid.CreateVersion7(),
            WeekStartDate = weekStartMonday,
            CreatedAt = DateTime.UtcNow,
        };

        await scheduleRepository.AddScheduleAsync(schedule, cancellationToken);
        await scheduleRepository.SaveChangesAsync(cancellationToken);

        // These sets are "soft preferences":
        // We try to avoid picking the same Leader/VIP multiple times in the same week,
        // but this must never block schedule creation.
        var pickLeaderThisWeek = new HashSet<Guid>();
        var pickVipThisWeek = new HashSet<Guid>();

        for (var i = 0; i < 7; i++)
        {
            var date = weekStartMonday.AddDays(i);

            // Leaders that were already tried today but could not form a valid Leader/VIP pair
            var triedLeadersToday = new HashSet<Guid>();
            Guid leaderId;
            Guid vipId;

            while (true)
            {
                // Exclude leaders that were already used this week
                // and leaders that were already tried today but failed to form a valid pair
                var leaderExclude = new HashSet<Guid>(pickLeaderThisWeek);
                foreach (var t in triedLeadersToday) leaderExclude.Add(t);

                // Pick a leader (ForcePick and pool logic are handled inside the repository)
                leaderId = await poolRepository.PickLeaderAsync(leaderExclude, cancellationToken);

                // Try to find a VIP that is not the same as the leader and respects weekly preferences
                var vipCandidate = await poolRepository.TryPickVipAsync(leaderId, pickVipThisWeek, cancellationToken);
                if (vipCandidate is not null)
                {
                    vipId = vipCandidate.Value;
                    // valid Leader/VIP pair found for this day
                    break;
                }

                // This leader could not form a valid pair today -> try a different leader
                triedLeadersToday.Add(leaderId);

                // Safety guard: if we tried "too many" different leaders and none worked,
                // something is wrong with the pool state or constraints.
                // This prevents an infinite loop in the weekly job.
                if (triedLeadersToday.Count > 100)
                    throw new InvalidOperationException("Could not find a valid Leader/VIP pair.");
            }

            // Remember that this leader and VIP were already used this week (soft preference)
            pickLeaderThisWeek.Add(leaderId);
            pickVipThisWeek.Add(vipId);

            // Mark both as used in the pool so fairness across cycles is guaranteed
            await poolRepository.MarkVipUsedAsync(vipId, cancellationToken);
            await poolRepository.MarkLeaderUsedAsync(leaderId, cancellationToken);

            // Persist the day entry
            await scheduleRepository.AddDayAsync(new WeeklyScheduleDay
            {
                Id = Guid.CreateVersion7(),
                ScheduleId = schedule.Id,
                Date = date,
                LeaderPlayerId = leaderId,
                VipPlayerId = vipId
            }, cancellationToken);

            await scheduleRepository.SaveChangesAsync(cancellationToken);
        }

        // Commit the whole week as one atomic operation
        await transaction.CommitAsync(cancellationToken);

        return (await scheduleRepository.GetWeeklyAsync(weekStartMonday, cancellationToken))!;
    }
}
