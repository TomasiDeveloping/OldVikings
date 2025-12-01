using OldVikings.Api.DataTransferObjects.ShinyServer;

namespace OldVikings.Api.Interfaces;

public interface IShinyServerRepository
{
    Task<ShinyServerDto> InsertShinyServerAsync(InsertShinyServerDto insertShinyServerDto, CancellationToken cancellationToken = default);

    Task<List<ShinyServerDto>> GetShinyServersAsync(CancellationToken cancellationToken = default);

    Task<List<ShinyServerDto>> GetShinyServersForTodayAsync(CancellationToken cancellationToken = default);
}