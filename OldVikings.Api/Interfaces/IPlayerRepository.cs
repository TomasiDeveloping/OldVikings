using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;

namespace OldVikings.Api.Interfaces;

public interface IPlayerRepository
{
    Task<List<PlayerDto>> GetPlayersAsync(CancellationToken cancellationToken = default);

    Task<int> CountActiveAsync(CancellationToken cancellationToken = default);

    Task<List<Guid>> GetActivePlayerIdsAsync(CancellationToken cancellationToken = default);

    Task<Player> CreateAsync(string playerName, CancellationToken cancellationToken = default);

    Task SetPlayerActiveAsync(Guid playerId, CancellationToken cancellationToken = default);

    Task DisablePlayerAsync(Guid playerId, CancellationToken cancellationToken = default);

    Task DeletePlayerAsync(Guid playerId, CancellationToken cancellationToken = default);
}