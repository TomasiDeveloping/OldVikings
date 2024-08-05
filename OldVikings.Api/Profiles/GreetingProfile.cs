using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.Greeting;

namespace OldVikings.Api.Profiles;

public class GreetingProfile : Profile
{
    public GreetingProfile()
    {
        CreateMap<Greeting, GreetingDto>();

        CreateMap<InsertGreetingDto, Greeting>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(des => des.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
    }
}