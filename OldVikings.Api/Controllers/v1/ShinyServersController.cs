using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.DataTransferObjects.ShinyServer;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShinyServersController(IShinyServerRepository repository, ILogger<ShinyServersController> logger) : ControllerBase
    {
        [HttpGet("ShinyServerToday")]
        public async Task<IActionResult> GetShinyServerTodayAsync(CancellationToken cancellationToken)
        {
            try
            {
                var shinyServerToday = await repository.GetShinyServersForTodayAsync(cancellationToken);
                return Ok(shinyServerToday);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving today's shiny server.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetShinyServersAsync(CancellationToken cancellationToken)
        {
            try
            {
                var shinyServers = await repository.GetShinyServersAsync(cancellationToken);
                return Ok(shinyServers);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving shiny servers.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddShinyServerAsync(InsertShinyServerDto shinyServerDto, CancellationToken cancellationToken)
        {
            try
            {
                var shinyServer = await repository.InsertShinyServerAsync(shinyServerDto, cancellationToken);
                return Ok(shinyServer);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a shiny server.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
