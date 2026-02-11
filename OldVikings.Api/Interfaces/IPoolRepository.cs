namespace OldVikings.Api.Interfaces;

public interface IPoolRepository
{
    Task SyncPoolsForActivePlayersAsync(CancellationToken cancellationToken = default);

    Task<Guid> PickLeaderAsync(HashSet<Guid> weekExclude, CancellationToken cancellationToken = default);
    Task<Guid?> TryPickVipAsync(Guid leaderId, HashSet<Guid> weekExclude, CancellationToken cancellationToken = default);

    Task MarkLeaderUsedAsync(Guid playerId, CancellationToken cancellationToken = default);
    Task MarkVipUsedAsync(Guid playerId, CancellationToken cancellationToken = default);

    Task SetLeaderForcePickAsync(Guid playerId, bool forcePick, CancellationToken cancellationToken = default);
    Task SetVipForcePickAsync(Guid playerId, bool forcePick, CancellationToken cancellationToken = default);

    Task BlockLeaderNextCycleAsync(Guid playerId, CancellationToken cancellationToken = default);
    Task BlockVipNextCycleAsync(Guid playerId, CancellationToken cancellationToken = default);
}