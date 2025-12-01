using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.ShinyServer;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class ShinyServerRepository(OldVikingsContext dbContext, IMapper mapper, ILogger<ShinyServerRepository> logger) : IShinyServerRepository
{
    public async Task<ShinyServerDto> InsertShinyServerAsync(InsertShinyServerDto insertShinyServerDto, CancellationToken cancellationToken = default)
    {
        var shinyServer = mapper.Map<ShinyServer>(insertShinyServerDto);

        try
        {
            await dbContext.ShinyServers.AddAsync(shinyServer, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ShinyServerDto>(shinyServer);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error inserting shiny server with server number {ServerNumber}", insertShinyServerDto.ServerNumber);
            throw new ApplicationException("An error occurred while inserting the shiny server.");
        }

    }

    public async Task<List<ShinyServerDto>> GetShinyServersAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.ShinyServers
            .ProjectTo<ShinyServerDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ShinyServerDto>> GetShinyServersForTodayAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        var candidates = await dbContext.ShinyServers
            .Where(server => server.FirstShinyDate <= today)
            .ProjectTo<ShinyServerDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var shinyServersToday = candidates
            .Where(server =>
            {
                var diffDays = today.DayNumber - server.FirstShinyDate.DayNumber;
                return diffDays % 3 == 0; 
            })
            .OrderBy(server => server.ServerNumber)
            .ToList();

        return shinyServersToday;
    }

}