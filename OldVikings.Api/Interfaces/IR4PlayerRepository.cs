using OldVikings.Api.Database.Entities;

namespace OldVikings.Api.Interfaces;

public interface IR4PlayerRepository
{
    Task<List<R4Player>> GetR4Players(CancellationToken cancellationToken = default);
}