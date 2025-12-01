using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.ShinyServer;

namespace OldVikings.Api.Profiles;

public class ShinyServerProfile : Profile
{
    public ShinyServerProfile()
    {
        CreateMap<ShinyServer, ShinyServerDto>();

        CreateMap<InsertShinyServerDto, ShinyServer>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(des => des.FirstShinyDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(DateTime.Now)));
    }
}