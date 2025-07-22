using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using OldVikings.Api.DataTransferObjects.TrainGuide;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Repositories;

public class TrainGuideRepository(OldVikingsContext context, ILogger<TrainGuideRepository> logger) : ITrainGuideRepository
{
    private string[] _players = [];
    public async Task<TrainGuideDto> GetTrainGuide(CancellationToken cancellationToken)
    {
        var trainGuide = await context.TrainGuides.FirstOrDefaultAsync(cancellationToken);

        if (trainGuide is null)
        {
            logger.LogError("No Data found");
            throw new ApplicationException("No Data found");
        }

        await GetR4Players(cancellationToken);

        var currentIndex = trainGuide.CurrentPlayerIndex;
        var today = DateTime.Now;
        string currentTrainLeader;
        string nextTrainLeader;
        string nextButOneTrainLeader;

        switch (today.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                currentTrainLeader = "1Place";
                nextTrainLeader = "2Place";
                nextButOneTrainLeader = "3Place";
                break;
            case DayOfWeek.Monday:
                currentTrainLeader = "2Place";
                nextTrainLeader = "3Place";
                nextButOneTrainLeader = "MVP";
                break;
            case DayOfWeek.Tuesday:
                currentTrainLeader = "3Place";
                nextTrainLeader = "MVP";
                nextButOneTrainLeader = _players[currentIndex];
                break;
            case DayOfWeek.Wednesday:
                currentTrainLeader = "MVP";
                nextTrainLeader = _players[currentIndex];
                nextButOneTrainLeader = _players[(currentIndex + 1) % _players.Length];
                break;
            case DayOfWeek.Thursday:
                currentTrainLeader = _players[currentIndex];
                nextTrainLeader = _players[(currentIndex + 1) % _players.Length];
                nextButOneTrainLeader = _players[(currentIndex + 2) % _players.Length];
                break;
            case DayOfWeek.Friday:
                currentTrainLeader = _players[currentIndex];
                nextTrainLeader = _players[(currentIndex + 1) % _players.Length];
                nextButOneTrainLeader = "1Place";
                break;
            case DayOfWeek.Saturday:
                currentTrainLeader = _players[currentIndex];
                nextTrainLeader = "2Place";
                nextButOneTrainLeader = "3Place";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        return new TrainGuideDto()
        {
            CurrentPlayer = currentTrainLeader,
            NextPlayer = nextTrainLeader,
            NextButOnePlayer = nextButOneTrainLeader,
        };
    }

    private async Task GetR4Players(CancellationToken cancellationToken)
    {
        var players = await context.R4Players
            .AsNoTracking()
            .OrderBy(r4Player => r4Player.Order)
            .ToListAsync(cancellationToken);

        _players = players.Select(p => p.PlayerName).ToArray();
    }
}