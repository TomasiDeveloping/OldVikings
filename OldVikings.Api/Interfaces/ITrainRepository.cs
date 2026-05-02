using OldVikings.Api.Dto.Train;

namespace OldVikings.Api.Interfaces;

public interface ITrainRepository
{
    Task<List<TrainConductorDto>> GetTrainConductorsAsync(CancellationToken cancellationToken = default);

    Task<List<TrainVipDto>> GetTrainVipsAsync(CancellationToken cancellationToken = default);

    Task<TrainVipDto> UpdateTrainVip(TrainVipDto dto, CancellationToken cancellationToken = default);

    Task<TrainConductorDto> UpdateTrainConductor(TrainConductorDto dto, CancellationToken cancellationToken = default);

}