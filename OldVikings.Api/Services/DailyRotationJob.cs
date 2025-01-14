using Microsoft.EntityFrameworkCore;
using OldVikings.Api.Database;
using Quartz;
using static Quartz.Logging.OperationName;
using System.Reflection.Metadata;

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

            if (today.DayOfWeek == DayOfWeek.Wednesday)
            {
                trainGuide.LastUpdate = today;
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Only the date has been updated as Wednesday");
                return;
            }

            var nextIndex = trainGuide.CurrentPlayerIndex++;

            if (nextIndex > 10) nextIndex = 0;

            trainGuide.CurrentPlayerIndex = nextIndex;
            trainGuide.LastUpdate = today;
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Successfully updated.New index = {trainGuide.CurrentPlayerIndex}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }

    }
}