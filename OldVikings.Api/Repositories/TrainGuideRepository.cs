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

        var currentIndex = trainGuide.CurrentPlayerIndex;
        var nextIndex = trainGuide.CurrentPlayerIndex;
        var nextButOneIndex = trainGuide.CurrentPlayerIndex;

        switch (today.DayOfWeek)
        {
            case DayOfWeek.Wednesday:
                nextButOneIndex += 1;
                break;
            case DayOfWeek.Thursday or DayOfWeek.Friday:
                nextIndex += 1;
                nextButOneIndex += 2;
                break;
        }

        return new TrainGuideDto()
        {
            CurrentPlayer = GetPlayerForDay(today, currentIndex),
            NextPlayer = GetPlayerForDay(today.AddDays(1), nextIndex),
            NextButOnePlayer = GetPlayerForDay(today.AddDays(2), nextButOneIndex),
        };
    }

    private string GetPlayerForDay(DateTime date, int index)
    {
        return _specialDays.TryGetValue(date.DayOfWeek, out var specialPlayer) 
            ? specialPlayer 
            : _players[index % _players.Length];
    }

    private readonly Dictionary<DayOfWeek, string> _specialDays = new()
    {
        { DayOfWeek.Wednesday, "MVP" },
        { DayOfWeek.Sunday , "1Place"},
        { DayOfWeek.Monday , "2Place"},
        { DayOfWeek.Tuesday , "3Place"}
    };

    private readonly string[] _players =
    [
        "forrgaet",
        "YLBH001",
        "ThorCH",
        "Faye AKA Dave",
        "Ljv26",
        "Hoppit",
        "Arvad",
        "Bjorngij",
        "JJ2327",
        "Rosscoz",
        "Kartoh"
    ];
}