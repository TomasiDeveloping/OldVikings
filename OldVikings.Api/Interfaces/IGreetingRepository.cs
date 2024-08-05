using OldVikings.Api.DataTransferObjects.Greeting;

namespace OldVikings.Api.Interfaces;

public interface IGreetingRepository
{
    Task<GreetingDto> InsertGreetingAsync(InsertGreetingDto insertGreetingDto, CancellationToken cancellationToken);

    Task<List<GreetingDto>> GetGreetingsAsync(CancellationToken cancellationToken);
}