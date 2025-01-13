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

        string currentPlayer;
        string nextPlayer;

        if (isWednesday)
        {
            currentPlayer = "MVP";
            nextPlayer = _players[trainGuide.CurrentPlayerIndex];
        }
        else
        {
            currentPlayer = _players[trainGuide.CurrentPlayerIndex];

            nextPlayer = _players[(trainGuide.CurrentPlayerIndex + 1) % _players.Length];
        }

        return new TrainGuideDto()
        {
            CurrentPlayer = currentPlayer,
            NextPlayer = nextPlayer,
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