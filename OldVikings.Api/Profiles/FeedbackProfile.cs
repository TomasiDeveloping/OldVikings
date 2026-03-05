using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.Feedback;
using OldVikings.Api.Helper;

namespace OldVikings.Api.Profiles;

public class FeedbackProfile : Profile
{
    public FeedbackProfile()
    {
        CreateMap<FeedbackItem, FeedbackDto>();

        CreateMap<CreateFeedbackDto, FeedbackItem>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => Guid.CreateVersion7()))
            .ForMember(des => des.CreatedAtUtc, opt => opt.MapFrom(scr => DateTime.UtcNow))
            .ForMember(des => des.IsAnonymous, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.DisplayName)))
            .ForMember(des => des.Status, opt => opt.MapFrom(scr => FeedbackStatus.New))
            .ForMember(des => des.VotesCount, opt => opt.MapFrom(scr => 0));

        CreateMap<UpdateFeedbackDto, FeedbackItem>()
            .ForMember(des => des.UpdatedAtUtc, opt => opt.MapFrom(src => DateTime.UtcNow));
    }
}