using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.TrainGuide;

namespace OldVikings.Api.Interfaces;

public interface ITrainGuideRepository
{
    Task<TrainGuideDto> GetTrainGuide(CancellationToken  cancellationToken);
}