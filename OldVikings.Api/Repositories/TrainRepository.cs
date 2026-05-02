using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Dto.Train;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class TrainRepository(OldVikingsContext dbContext, IMapper mapper) : ITrainRepository
{
    public async Task<List<TrainConductorDto>> GetTrainConductorsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.PoolLeaders
            .AsNoTracking()
            .ProjectTo<TrainConductorDto>(mapper.ConfigurationProvider)
            .OrderBy(x => x.PlayerName)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<TrainVipDto>> GetTrainVipsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.PoolVips
            .AsNoTracking()
            .ProjectTo<TrainVipDto>(mapper.ConfigurationProvider)
            .OrderBy(x => x.PlayerName)
            .ToListAsync(cancellationToken);
    }

    public async Task<TrainVipDto> UpdateTrainVip(TrainVipDto dto, CancellationToken cancellationToken = default)
    {
        var vip = await dbContext.PoolVips.FirstOrDefaultAsync(x => x.PlayerId == dto.PlayerId, cancellationToken);

        if (vip is null)
        {
            throw new Exception($"VIP with ID {dto.PlayerId} not found.");
        }

        vip.BlockNextCycle = dto.BlockNextCycle;
        vip.ForcePick = dto.ForcePick;

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<TrainVipDto>(vip);
    }

    public async Task<TrainConductorDto> UpdateTrainConductor(TrainConductorDto dto, CancellationToken cancellationToken = default)
    {
        var conductor = await dbContext.PoolLeaders.FirstOrDefaultAsync(x => x.PlayerId == dto.PlayerId, cancellationToken);

        if (conductor is null)
        {
            throw new Exception($"Conductor with ID {dto.PlayerId} not found.");
        }

        conductor.BlockNextCycle = dto.BlockNextCycle;
        conductor.ForcePick = dto.ForcePick;

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<TrainConductorDto>(conductor);
    }
}