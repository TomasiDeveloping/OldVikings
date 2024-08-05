using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.DataTransferObjects.Greeting;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class GreetingRepository(IMapper mapper, OldVikingsContext context) : IGreetingRepository
{
    public async Task<GreetingDto> InsertGreetingAsync(InsertGreetingDto insertGreetingDto, CancellationToken cancellationToken)
    {
        var newGreeting = mapper.Map<Greeting>(insertGreetingDto);
        await context.Greetings.AddAsync(newGreeting, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<GreetingDto>(newGreeting);
    }

    public async Task<List<GreetingDto>> GetGreetingsAsync(CancellationToken cancellationToken)
    {
        var greetings = await context.Greetings
            .ProjectTo<GreetingDto>(mapper.ConfigurationProvider)
            .OrderByDescending(greeting => greeting.CreatedAt)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return greetings;
    }
}