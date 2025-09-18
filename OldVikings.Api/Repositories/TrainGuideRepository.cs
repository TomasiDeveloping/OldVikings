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
        var trainGuide = await context.TrainGuides.FirstOrDefaultAsync(cancellationToken)
                         ?? throw new ApplicationException("No Data found");

        await LoadPlayersAsync(cancellationToken);

        if (_players.Length == 0)
            throw new ApplicationException("Keine R4-Spieler gefunden");

        // Index sicher normalisieren (funktioniert auch bei negativen Werten)
        var idx = NormalizeIndex(trainGuide.CurrentPlayerIndex, _players.Length);

        // Zyklischer Zugriff: At(0) = aktueller, At(1) = nächster, At(2) = übernächster
        string At(int offset) => _players[NormalizeIndex(idx + offset, _players.Length)];

        var today = DateTime.Now.DayOfWeek;

        string currentTrainLeader;
        string nextTrainLeader;
        string nextButOneTrainLeader;

        switch (today)
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
                nextButOneTrainLeader = At(0);
                break;
            case DayOfWeek.Wednesday:
                currentTrainLeader = "MVP";
                nextTrainLeader = At(0);
                nextButOneTrainLeader = At(1);
                break;
            case DayOfWeek.Thursday:
                currentTrainLeader = At(0);
                nextTrainLeader = At(1);
                nextButOneTrainLeader = At(2);
                break;
            case DayOfWeek.Friday:
                currentTrainLeader = At(0);
                nextTrainLeader = At(1);
                nextButOneTrainLeader = "1Place";
                break;
            case DayOfWeek.Saturday:
                currentTrainLeader = At(0);
                nextTrainLeader = "2Place";
                nextButOneTrainLeader = "3Place";
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new TrainGuideDto
        {
            CurrentPlayer = currentTrainLeader,
            NextPlayer = nextTrainLeader,
            NextButOnePlayer = nextButOneTrainLeader,
        };
    }

    private async Task LoadPlayersAsync(CancellationToken ct)
    {
        _players = await context.R4Players
            .AsNoTracking()
            .OrderBy(p => p.Order)
            .Select(p => p.PlayerName)
            .ToArrayAsync(ct);
    }

    private static int NormalizeIndex(int index, int length)
    {
        // Modulo, das auch bei negativen Werten korrekt in [0, length-1] liegt
        var m = index % length;
        return m < 0 ? m + length : m;
    }
}
