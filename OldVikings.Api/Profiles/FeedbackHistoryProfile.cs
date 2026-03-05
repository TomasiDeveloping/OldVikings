using AutoMapper;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.FeedbackHistory;

namespace OldVikings.Api.Profiles;

public class FeedbackHistoryProfile : Profile
{
    public FeedbackHistoryProfile()
    {
        CreateMap<FeedbackStatusHistory, FeedbackHistoryDto>();
    }
}