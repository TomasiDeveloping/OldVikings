using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.DataTransferObjects.TrainGuide;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class TrainGuidesController(ITrainGuideRepository trainGuideRepository, ILogger<TrainGuidesController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TrainGuideDto>> GetTrainGuide(CancellationToken cancellationToken)
    {
        try
        {
            return Ok(await trainGuideRepository.GetTrainGuide(cancellationToken));
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}