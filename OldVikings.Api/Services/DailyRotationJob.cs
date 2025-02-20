using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using Quartz;

namespace OldVikings.Api.Services;

public class DailyRotationJob(OldVikingsContext dbContext, ILogger<DailyRotationJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var today = DateTime.Now;
        logger.LogInformation($"Job {nameof(DailyRotationJob)} is executed: {today}");

        try
        {
            var trainGuide = await dbContext.TrainGuides.FirstOrDefaultAsync();

            if (trainGuide is null)
            {
                logger.LogError("Train guide ist null");
                return;
            }

            if (trainGuide.LastUpdate.Date == today.Date)
            {
                logger.LogInformation($"Train guide has already been processed at: {trainGuide.LastUpdate}");
                return;
            }

            if (today.DayOfWeek is DayOfWeek.Thursday or DayOfWeek.Friday or DayOfWeek.Saturday)
            {
                var nextIndex = trainGuide.CurrentPlayerIndex + 1;

                if (nextIndex > 10) nextIndex = 0;

                trainGuide.CurrentPlayerIndex = nextIndex;
                logger.LogInformation($"Index updated. New index = {trainGuide.CurrentPlayerIndex}");
            }

            trainGuide.LastUpdate = today;
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Successfully saved changes. LastUpdate = {trainGuide.LastUpdate}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}