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
        string nextButOnePlayer;
        var currentIndex = trainGuide.CurrentPlayerIndex;
        var nextIndex = trainGuide.CurrentPlayerIndex + 1;
        var nextButOneIndex = trainGuide.CurrentPlayerIndex + 2;

        if (isWednesday)
        {
            currentPlayer = "MVP";
            nextPlayer = _players[nextIndex > 10 ? 0 : nextIndex];
            nextButOnePlayer = _players[nextButOneIndex > 10 ? 0 : nextButOneIndex];
        }
        else
        {
            currentPlayer = _players[currentIndex];

            nextPlayer = today.AddDays(1).DayOfWeek == DayOfWeek.Wednesday ? "MVP" : _players[nextIndex > 10 ? 0 : nextIndex];

            nextButOnePlayer = today.AddDays(2).DayOfWeek == DayOfWeek.Wednesday
                ? "MVP"
                : _players[nextButOneIndex > 10 ? 0 : nextButOneIndex];
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