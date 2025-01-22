using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.DataTransferObjects.TrainGuide;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class TrainGuideRepository(OldVikingsContext context, ILogger<TrainGuideRepository> logger) : ITrainGuideRepository
{
    public async Task<TrainGuideDto> GetTrainGuide(CancellationToken cancellationToken)
    {
        var trainGuide = await context.TrainGuides.FirstOrDefaultAsync(cancellationToken);

        if (trainGuide is null)
        {
            logger.LogError("No Data found");
            throw new ApplicationException("No Data found");
        }

        var today = DateTime.Now;
        var isWednesday = today.DayOfWeek == DayOfWeek.Wednesday;

        var currentIndex = trainGuide.CurrentPlayerIndex;
        var nextIndex = (currentIndex + 1) % _players.Length;
        var nextButOneIndex = (currentIndex + 2) % _players.Length;

        var currentPlayer = _players[currentIndex];
        var nextPlayer = _players[nextIndex];
        var nextButOnePlayer = _players[nextButOneIndex];

        if (isWednesday)
        {
            currentPlayer = "MVP";
        }
        else
        {
            if (today.AddDays(1).DayOfWeek == DayOfWeek.Wednesday)
            {
                nextPlayer = "MVP";
                nextButOnePlayer = _players[nextIndex];
            }

            if (today.AddDays(2).DayOfWeek == DayOfWeek.Wednesday)
            {
                nextButOnePlayer = "MVP";
            }

        }

        return new TrainGuideDto()
        {
            CurrentPlayer = currentPlayer,
            NextPlayer = nextPlayer,
            NextButOnePlayer = nextButOnePlayer,
        };
    }

    private readonly string[] _players =
    [
        "forrgaet",
        "MoonDust",
        "ThorCH",
        "YLBH001",
        "Ljv26",
        "Hoppit",
        "Arvad",
        "Bjorngij",
        "JJ2327",
        "Rosscoz",
        "Kartoh"
    ];
}