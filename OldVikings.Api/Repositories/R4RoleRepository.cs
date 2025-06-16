using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.DataTransferObjects.R4Roles;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class R4RoleRepository(OldVikingsContext dbContext, IMapper mapper, ILogger<R4RoleRepository> logger) : IR4RoleRepository
{
    public async Task<List<R4RoleDto>> GetR4Roles(CancellationToken cancellationToken = default)
    {
        var r4Roles = await dbContext.R4Roles
            .ProjectTo<R4RoleDto>(mapper.ConfigurationProvider)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return r4Roles;
    }

    public async Task<R4RoleDto> UpdateR4Role(R4RoleDto r4RoleDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var roleToUpdate = await dbContext.R4Roles.FirstOrDefaultAsync(r => r.Id == r4RoleDto.Id, cancellationToken);

            if (roleToUpdate == null)
            {
                logger.LogError($"Role with ID {r4RoleDto.Id} not found for update.");
                throw new KeyNotFoundException($"Role with ID {r4RoleDto.Id} not found.");
            }

            mapper.Map(r4RoleDto, roleToUpdate);

            await dbContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<R4RoleDto>(roleToUpdate);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}