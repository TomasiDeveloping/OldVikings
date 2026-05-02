using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto.Train;

namespace OldVikings.Api.Profiles;

public class TrainProfile : Profile
{
    public TrainProfile()
    {
        CreateMap<PoolLeader, TrainConductorDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.DisplayName));

        CreateMap<PoolVip, TrainVipDto>()
            .ForMember(des => des.PlayerName, opt => opt.MapFrom(src => src.Player.DisplayName));
    }
}