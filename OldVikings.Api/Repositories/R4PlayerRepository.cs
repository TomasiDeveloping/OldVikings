using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class R4PlayerRepository(OldVikingsContext dbContext) : IR4PlayerRepository
{
    public async Task<List<R4Player>> GetR4Players(CancellationToken cancellationToken = default)
    {
        return await dbContext.R4Players
            .AsNoTracking()
            .OrderBy(d => d.Order)
            .ToListAsync(cancellationToken);
    }
}