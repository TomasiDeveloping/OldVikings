using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OldVikings.Api.Classes;
using OldVikings.Api.Database;
using OldVikings.Api.Services;
using System.Text;

namespace OldVikings.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class WebHooksController(ILogger<WebHooksController> logger, HttpClient httpClient, IOptions<DiscordWebhookOptions> options, OldVikingsContext dbContext) : ControllerBase
{


    private readonly DiscordWebhookOptions _discordWebhookOptions = options.Value;

    [HttpGet("rotation")]
    public async Task<IActionResult> RotateTrain()
    {
        try
        {
            var trainGuide = await dbContext.TrainGuides.FirstOrDefaultAsync();

            if (trainGuide is null)
            {
                logger.LogError("Train guide ist null");
                return BadRequest("Train guide is null");
            }

            var playerCount = await dbContext.R4Players.CountAsync();
            if (playerCount == 0)
            {
                logger.LogError("No players found in R4Players table");
                return BadRequest("No players found in R4Players table");
            }


            var nextIndex = (trainGuide.CurrentPlayerIndex + 1) % playerCount;


            trainGuide.CurrentPlayerIndex = nextIndex;
            trainGuide.LastUpdate = DateTime.Now;

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Successfully saved changes. LastUpdate = {trainGuide.LastUpdate}");

            return Ok();
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return BadRequest(e.Message);
        }
    }

    [HttpGet("marshal")]
    public async Task<IActionResult> MarshalReminder([FromQuery] string key)
    {
        try
        {
            if (key != _discordWebhookOptions.ApiKey)
            {
                return Unauthorized();
            }

            var startDate = new DateTime(2025, 5, 23);
            var today = DateTime.Today;

            var dayDifference = (today - startDate).Days;

            if (dayDifference % 2 != 0) return Accepted();

            var jsonBody = GetMarshalContent();

            var response = await SendToDiscordWebhook(_discordWebhookOptions.EventChannelUrl, jsonBody);

            return response.IsSuccessStatusCode
                ? Accepted()
                : Problem(statusCode: (int)response.StatusCode,
                    detail: $"Error posting to webhook: {response.ReasonPhrase}",
                    title: "Webhook Error");

        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExceptionMessage}", e.Message);
            return Problem(statusCode: StatusCodes.Status500InternalServerError,
                detail: $"Error on {nameof(MarshalReminder)}", title: "Internal Server Error");
        }
    }

    [HttpGet("zombie-siege")]
    public async Task<IActionResult> ZombieSiegeReminder([FromQuery]string key, [FromQuery]int level = 7, [FromQuery]int baseLevel = 26)
    {
        try
        {
            if (key != _discordWebhookOptions.ApiKey)
            {
                return Unauthorized();
            }

            var today = DateTime.Today;

            if (!ShouldZombieSiegeTrigger(today))
            {
                return Accepted();
            }

            var jsonBody = GetZombieSiegeContent(level, baseLevel);

            var response = await SendToDiscordWebhook(_discordWebhookOptions.EventChannelUrl, jsonBody);

            return response.IsSuccessStatusCode
                ? Accepted()
                : Problem(statusCode: (int)response.StatusCode,
                    detail: $"Error posting to webhook: {response.ReasonPhrase}",
                    title: "Webhook Error");
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExceptionMessage}", e.Message);
            return Problem(statusCode: StatusCodes.Status500InternalServerError,
                detail: $"Error on {nameof(ZombieSiegeReminder)}", title: "Internal Server Error");
        }
    }

    [HttpGet("desert-storm")]
    public async Task<IActionResult> DesertStormReminder([FromQuery]string key, string team)
    {
        try
        {
            if (key != _discordWebhookOptions.ApiKey)
            {
                return Unauthorized();
            }

            var jsonBody = GetDesertStormContent(team);

            var response = await SendToDiscordWebhook(_discordWebhookOptions.DesertStormChannelUrl, jsonBody);

            return response.IsSuccessStatusCode
                ? Accepted()
                : Problem(statusCode: (int)response.StatusCode,
                    detail: $"Error posting to webhook: {response.ReasonPhrase}",
                    title: "Webhook Error");
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExceptionMessage}", e.Message);
            return Problem(statusCode: StatusCodes.Status500InternalServerError,
                detail: $"Error on {nameof(DesertStormReminder)}", title: "Internal Server Error");
        }
    }

    [HttpGet("train-start")]
    public async Task<IActionResult> TrainStartReminder([FromQuery]string key)
    {
        try
        {
            if (key != _discordWebhookOptions.ApiKey)
            {
                return Unauthorized();
            }

            var jsonBody = GetTrainStartContent();

            var response = await SendToDiscordWebhook(_discordWebhookOptions.TrainChannelUrl, jsonBody);

            return response.IsSuccessStatusCode
                ? Accepted()
                : Problem(statusCode: (int)response.StatusCode,
                    detail: $"Error posting to webhook: {response.ReasonPhrase}",
                    title: "Webhook Error");
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExceptionMessage}", e.Message);
            return Problem(statusCode: StatusCodes.Status500InternalServerError,
                detail: $"Error on {nameof(TrainStartReminder)}", title: "Internal Server Error");
        }
    }

    [HttpGet("train-departure")]
    public async Task<IActionResult> TrainDepartureReminder([FromQuery]string key)
    {
        try
        {
            if (key != _discordWebhookOptions.ApiKey)
            {
                return Unauthorized();
            }

            var jsonBody = GetTrainDepartureContent();

            var response = await SendToDiscordWebhook(_discordWebhookOptions.TrainChannelUrl, jsonBody);

            return response.IsSuccessStatusCode
                ? Accepted()
                : Problem(statusCode: (int)response.StatusCode,
                    detail: $"Error posting to webhook: {response.ReasonPhrase}",
                    title: "Webhook Error");
        }
        catch (Exception e)
        {
            logger.LogError(e, "{ExceptionMessage}", e.Message);
            return Problem(statusCode: StatusCodes.Status500InternalServerError,
                detail: $"Error on {nameof(TrainDepartureReminder)}", title: "Internal Server Error");
        }
    }

    private async Task<HttpResponseMessage> SendToDiscordWebhook(string url, string jsonBody)
    {
        return await httpClient.PostAsync(url, GetStringContent(jsonBody));
    }

    private static StringContent GetStringContent(string jsonBody)
    {
        return new StringContent(jsonBody, Encoding.UTF8, "application/json");
    }

    private static bool ShouldZombieSiegeTrigger(DateTime today)
    {
        if (today.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        var baseDate = new DateTime(2025, 5, 28);

        var weeksSinceStart = (int)((today.Date - baseDate.Date).TotalDays / 7);

        var isEvenWeek = weeksSinceStart % 2 == 0;

        switch (isEvenWeek)
        {
            case true when today.DayOfWeek == DayOfWeek.Wednesday:
            case false when today.DayOfWeek == DayOfWeek.Thursday:
                return true;
            default:
                return false;
        }
    }

    private static string GetMarshalContent()
    {
        return """

               {
                 "content": "@everyone",
                 "embeds": [
                   {
                     "title": "🛡️ Incoming: Marshal Event",
                     "description": "This is your 15-minute warning. The **Marshal** alliance event is almost here.\n\nGear up, gather your squad, and get ready to make your mark.\n\nI’m just the messenger – but victory? That’s on you.",
                     "color": 16098851,
                     "footer": {
                       "text": "Marshal begins soon. Be ready."
                     }
                   }
                 ]
               }
               """;
    }

    private static string GetZombieSiegeContent(int zombieLevel, int baseLevel)
    {
        return $$"""

                 {
                   "content": "@everyone",
                   "embeds": [
                     {
                       "title": "🧟‍♂️ Zombie Siege Level {{zombieLevel}} begins in 15 minutes!",
                       "description": "**Bases level {{baseLevel}} and above will be attacked.**\nMan your walls – **shields won't help you!**\n\nGet ready, commanders. The undead are closing in…",
                       "color": 15158332,
                       "footer": {
                         "text": "Zombie Siege – Defend your base!"
                       }
                     }
                   ]
                 }
                 """;
    }

    private static string GetDesertStormContent(string team)
    {
        return $$"""

               {
                 "content": "@everyone",
                 "embeds": [
                   {
                     "title": "🌪️ Desert Storm for Team {{team}} starts in 15 minutes!",
                     "description": "Team {{team}} **All registered players must be on time.**\nBackup players – stay alert and be ready to step in if needed.\n\nStick to the battle plan, stay focused, and let’s give it our best.\n\n_We fight as one – from first wave to final push._",
                     "color": 15844367,
                     "footer": {
                       "text": "Desert Storm – Stay sharp and coordinated."
                     }
                   }
                 ]
               }
               """;
    }

    private static string GetTrainStartContent()
    {
        return """

               {
                 "content": "@everyone",
                 "embeds": [
                   {
                     "title": "🚆 Train assignment incoming",
                     "description": "The train will be assigned shortly.\n\nOnce it's live, you’ll have **4 hours** to apply.\nSpots will be distributed **randomly** among all applicants.\n\nPlease make sure to apply within the time window.",
                     "color": 3447003,
                     "footer": {
                       "text": "Train – 4 hours to apply after assignment."
                     }
                   }
                 ]
               }
               """;
    }

    private static string GetTrainDepartureContent()
    {
        return """

               {
                 "content": "@everyone",
                 "embeds": [
                   {
                     "title": "⏳ Final reminder – Train applications closing soon!",
                     "description": "Only a short time left to apply for the current train.\n\nIf you haven't submitted your application yet, do it now.\n\nOnce the window closes, no more entries will be accepted.",
                     "color": 15105570,
                     "footer": {
                       "text": "Train – Last chance to apply."
                     }
                   }
                 ]
               }
               """;
    }
}