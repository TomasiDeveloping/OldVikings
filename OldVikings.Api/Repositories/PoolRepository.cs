using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class PoolRepository(OldVikingsContext dbContext) : IPoolRepository
{
    public async Task SyncPoolsForActivePlayersAsync(CancellationToken cancellationToken = default)
    {
        // Ensures that every registered player exists in both pools (Leader + VIP).
        // Newly registered players are automatically added to the pools.
        var registeredIds = await dbContext.Players
            .Where(player => player.Registered)
            .Select(player => player.Id)
            .ToListAsync(cancellationToken);

        var leaderIds = await dbContext.PoolLeaders.Select(poolLeader => poolLeader.PlayerId).ToListAsync(cancellationToken);
        var vipIds = await dbContext.PoolVips.Select(poolVip => poolVip.PlayerId).ToListAsync(cancellationToken);

        var missingLeaderIds = registeredIds.Except(leaderIds).ToList();
        var missingVipIds = registeredIds.Except(vipIds).ToList();

        foreach (var leaderId in missingLeaderIds)
        {
            await dbContext.PoolLeaders.AddAsync(new PoolLeader
            {
                PlayerId = leaderId,
                IsAvailable = true,
                ForcePick = false,
                BlockNextCycle = false,
                UpdatedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        foreach (var vipId in missingVipIds)
        {
            await dbContext.PoolVips.AddAsync(new PoolVip
            {
                PlayerId = vipId,
                IsAvailable = true,
                ForcePick = false,
                BlockNextCycle = false,
                UpdatedAt = DateTime.UtcNow
            }, cancellationToken);
        }

        if (missingLeaderIds.Count > 0 || missingVipIds.Count > 0)
            await dbContext.SaveChangesAsync(cancellationToken);
    }

    // Marks a player as "force pick" for the next schedule creation.
    // ForcePick means the player will be preferred over normal candidates.
    public async Task SetLeaderForcePickAsync(Guid playerId, bool forcePick, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolLeaders
            .Where(poolLeader => poolLeader.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ForcePick, forcePick)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Marks a player as "force pick" for the next schedule creation (VIP pool).
    public async Task SetVipForcePickAsync(Guid playerId, bool forcePick, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolVips
            .Where(poolVip => poolVip.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ForcePick, forcePick)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Blocks a player for exactly the next refill cycle.
    // The player will be skipped once and then automatically re-enabled.
    public async Task BlockLeaderNextCycleAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolLeaders
            .Where(x => x.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.BlockNextCycle, true)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Blocks a VIP player for exactly the next refill cycle.
    public async Task BlockVipNextCycleAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolVips
            .Where(x => x.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.BlockNextCycle, true)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Picks a leader for a single day.
    // Priority:
    //   1) ForcePick (manual override)
    //   2) Normal pool respecting weekly preferences
    //   3) Fallback ignoring weekly preferences (never block schedule creation)
    public async Task<Guid> PickLeaderAsync(HashSet<Guid> weekExclude, CancellationToken cancellationToken = default)
    {
        // Make sure the pool is in a valid state before picking
        await EnsureLeaderEligibleAsync(cancellationToken);

        var tried = new HashSet<Guid>();

        while (true)
        {
            // 1) ForcePick: ignore IsAvailable and weekly exclusions
            var forced = await PickLeaderCandidateAsync(forceOnly: true, weekExclude: null, tried, cancellationToken);
            if (forced is not null)
            {
                if (forced.Value.Approved) return forced.Value.PlayerId;

                // Player is currently blocked (Approved=false) -> skip for this cycle
                await MarkLeaderUsedAsync(forced.Value.PlayerId, cancellationToken);
                tried.Add(forced.Value.PlayerId);
                continue;
            }

            // 2) Normal pick: respect weekly exclusions and fairness pool
            var normal = await PickLeaderCandidateAsync(forceOnly: false, weekExclude, tried, cancellationToken);
            if (normal is not null)
            {
                if (normal.Value.Approved) return normal.Value.PlayerId;

                // Player is temporarily not approved -> skip for this cycle
                await MarkLeaderUsedAsync(normal.Value.PlayerId, cancellationToken);
                tried.Add(normal.Value.PlayerId);
                continue;
            }

            // 3) Fallback: ignore weekly exclusions to guarantee a result
            var fallback = await PickLeaderCandidateAsync(forceOnly: false, weekExclude: null, tried, cancellationToken);
            if (fallback is not null)
            {
                if (fallback.Value.Approved) return fallback.Value.PlayerId;

                await MarkLeaderUsedAsync(fallback.Value.PlayerId, cancellationToken);
                tried.Add(fallback.Value.PlayerId);
                continue;
            }

            // Pool exhausted -> refill (round-robin reset) and retry
            await EnsureLeaderEligibleAsync(cancellationToken);
            tried.Clear();

            // Safety guard to avoid infinite loops in case of corrupted data
            if (tried.Count > 300)
                throw new InvalidOperationException("No approved leader available.");
        }
    }

    // Picks a VIP for a given leader.
    // Leader and VIP must not be the same player.
    public async Task<Guid?> TryPickVipAsync(Guid leaderId, HashSet<Guid> weekExclude, CancellationToken cancellationToken = default)
    {
        await EnsureVipEligibleAsync(cancellationToken);

        var tried = new HashSet<Guid>();

        while (true)
        {
            // 1) ForcePick: ignore IsAvailable and weekly exclusions
            var forced = await PickVipCandidateAsync(leaderId, forceOnly: true, weekExclude: null, tried, cancellationToken);
            if (forced is not null)
            {
                if (forced.Value.Approved) return forced.Value.PlayerId;

                await MarkVipUsedAsync(forced.Value.PlayerId, cancellationToken);
                tried.Add(forced.Value.PlayerId);
                continue;
            }

            // 2) Normal pick
            var normal = await PickVipCandidateAsync(leaderId, forceOnly: false, weekExclude, tried, cancellationToken);
            if (normal is not null)
            {
                if (normal.Value.Approved) return normal.Value.PlayerId;

                await MarkVipUsedAsync(normal.Value.PlayerId, cancellationToken);
                tried.Add(normal.Value.PlayerId);
                continue;
            }

            // 3) Fallback
            var fallback = await PickVipCandidateAsync(leaderId, forceOnly: false, weekExclude: null, tried, cancellationToken);
            if (fallback is not null)
            {
                if (fallback.Value.Approved) return fallback.Value.PlayerId;

                await MarkVipUsedAsync(fallback.Value.PlayerId, cancellationToken);
                tried.Add(fallback.Value.PlayerId);
                continue;
            }
            // No valid VIP available for this leader
            return null;
        }
    }

    // Marks a leader as used in the current round-robin cycle.
    // This guarantees fair distribution across weeks.
    public async Task MarkLeaderUsedAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolLeaders
            .Where(x => x.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, false)
                // ForcePick is a one-shot override
                .SetProperty(x => x.ForcePick, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Marks a VIP as used in the current round-robin cycle.
    public async Task MarkVipUsedAsync(Guid playerId, CancellationToken cancellationToken = default)
    {
        await dbContext.PoolVips
            .Where(x => x.PlayerId == playerId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, false)
                // ForcePick is a one-shot override
                .SetProperty(x => x.ForcePick, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Ensures the Leader pool is in a valid state.
    // If all eligible leaders are exhausted, the pool is refilled.
    private async Task EnsureLeaderEligibleAsync(CancellationToken cancellationToken)
    {
        // Deactivate players that are no longer registered
        await dbContext.PoolLeaders
            .Join(dbContext.Players.Where(p => !p.Registered),
                pl => pl.PlayerId, p => p.Id,
                (pl, p) => pl)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, false)
                .SetProperty(x => x.ForcePick, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);

        // Check if there is at least one available and approved leader left
        var anyApprovedAvailable = await dbContext.PoolLeaders
            .Where(pl => pl.IsAvailable)
            .Join(dbContext.Players.Where(p => p.Registered && p.Approved),
                pl => pl.PlayerId, p => p.Id,
                (pl, p) => pl.PlayerId)
            .AnyAsync(cancellationToken);

        if (anyApprovedAvailable) return;


        // Refill the pool (round-robin reset):
        // - all registered players become available again
        // - except players blocked for the next cycle (BlockNextCycle)
        // - BlockNextCycle is automatically cleared
        await dbContext.PoolLeaders
            .Join(dbContext.Players.Where(p => p.Registered),
                pl => pl.PlayerId, p => p.Id,
                (pl, p) => pl)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, x => !x.BlockNextCycle)
                .SetProperty(x => x.BlockNextCycle, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Ensures the VIP pool is in a valid state.
    private async Task EnsureVipEligibleAsync(CancellationToken cancellationToken)
    {
        // Deactivate players that are no longer registered
        await dbContext.PoolVips
            .Join(dbContext.Players.Where(p => !p.Registered),
                pv => pv.PlayerId, p => p.Id,
                (pv, p) => pv)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, false)
                .SetProperty(x => x.ForcePick, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);

        // Check if there is at least one available and approved VIP left
        var anyApprovedAvailable = await dbContext.PoolVips
            .Where(pv => pv.IsAvailable)
            .Join(dbContext.Players.Where(p => p.Registered && p.Approved),
                pv => pv.PlayerId, p => p.Id,
                (pv, p) => pv.PlayerId)
            .AnyAsync(cancellationToken);

        if (anyApprovedAvailable) return;

        // Refill the pool (round-robin reset)
        await dbContext.PoolVips
            .Join(dbContext.Players.Where(p => p.Registered),
                pv => pv.PlayerId, p => p.Id,
                (pv, p) => pv)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.IsAvailable, x => !x.BlockNextCycle)
                .SetProperty(x => x.BlockNextCycle, false)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow), cancellationToken);
    }

    // Helper: picks a single leader candidate and returns both the player id and approval state
    private async Task<(Guid PlayerId, bool Approved)?> PickLeaderCandidateAsync(
        bool forceOnly,
        HashSet<Guid>? weekExclude,
        HashSet<Guid> tried,
        CancellationToken cancellationToken)
    {
        var q = dbContext.PoolLeaders
            // ForcePick ignores IsAvailable to give "missed" players another chance
            .Where(pl => (forceOnly ? pl.ForcePick : pl.IsAvailable))
            .Join(dbContext.Players.Where(p => p.Registered),
                pl => pl.PlayerId, p => p.Id,
                (pl, p) => new { pl.PlayerId, p.Approved });

        if (weekExclude is not null && weekExclude.Count > 0)
            q = q.Where(x => !weekExclude.Contains(x.PlayerId));

        if (tried.Count > 0)
            q = q.Where(x => !tried.Contains(x.PlayerId));

        var row = await q.OrderBy(_ => Guid.NewGuid()).FirstOrDefaultAsync(cancellationToken);
        return row is null ? null : (row.PlayerId, row.Approved);
    }

    // Helper: picks a single VIP candidate (Leader must not be the same player)
    private async Task<(Guid PlayerId, bool Approved)?> PickVipCandidateAsync(
        Guid leaderId,
        bool forceOnly,
        HashSet<Guid>? weekExclude,
        HashSet<Guid> tried,
        CancellationToken cancellationToken)
    {
        var q = dbContext.PoolVips
            .Where(pv => (forceOnly ? pv.ForcePick : pv.IsAvailable))
            .Join(dbContext.Players.Where(p => p.Registered),
                pv => pv.PlayerId, p => p.Id,
                (pv, p) => new { pv.PlayerId, p.Approved })
            .Where(x => x.PlayerId != leaderId);

        if (weekExclude is not null && weekExclude.Count > 0)
            q = q.Where(x => !weekExclude.Contains(x.PlayerId));

        if (tried.Count > 0)
            q = q.Where(x => !tried.Contains(x.PlayerId));

        var row = await q.OrderBy(_ => Guid.NewGuid()).FirstOrDefaultAsync(cancellationToken);
        return row is null ? null : (row.PlayerId, row.Approved);
    }
}
