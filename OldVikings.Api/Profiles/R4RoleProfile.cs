using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.R4Roles;

namespace OldVikings.Api.Profiles;

public class R4RoleProfile : Profile
{
    public R4RoleProfile()
    {
        CreateMap<R4Roles, R4RoleDto>();
    }
}