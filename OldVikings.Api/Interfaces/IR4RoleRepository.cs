using OldVikings.Api.DataTransferObjects.R4Roles;

namespace OldVikings.Api.Interfaces;

public interface IR4RoleRepository
{
    Task<List<R4RoleDto>> GetR4Roles(CancellationToken cancellationToken = default);

    Task<R4RoleDto> UpdateR4Role(R4RoleDto r4RoleDto, CancellationToken cancellationToken = default);
}