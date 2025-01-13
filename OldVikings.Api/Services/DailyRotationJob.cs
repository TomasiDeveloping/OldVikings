using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;

namespace OldVikings.Api.Services;

public class DailyRotationJob(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = now.Date.AddDays(1).AddMinutes(1);
            var delay = nextRun - now;

            await Task.Delay(delay, stoppingToken);

            await UpdateRotation(stoppingToken);

            var emergencyRun = nextRun.Date.AddHours(3);
            delay = emergencyRun - DateTime.Now;

            if (delay <= TimeSpan.Zero) continue;
            await Task.Delay(delay, stoppingToken);
            await UpdateRotation(stoppingToken);

        }
    }

    private async Task UpdateRotation(CancellationToken stoppingToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<OldVikingsContext>();

        var trainGuide = await context.TrainGuides.FirstOrDefaultAsync(stoppingToken);

        if (trainGuide is not null)
        {
            if (trainGuide.LastUpdate.Date != DateTime.Now)
            {
                if (DateTime.Now.DayOfWeek != DayOfWeek.Wednesday)
                {
                    trainGuide.CurrentPlayerIndex = (trainGuide.CurrentPlayerIndex + 1) % 11;
                }

                trainGuide.LastUpdate = DateTime.Now;

                await context.SaveChangesAsync(stoppingToken);
            }
        }
    }
}