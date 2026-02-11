using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.Dto;
using OldVikings.Api.Interfaces;


namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchedulesController(IScheduleRepository scheduleRepository) : ControllerBase
    {
        [HttpGet("current-week")]
        public async Task<ActionResult<WeeklyScheduleDto>> GetCurrentWeek(CancellationToken cancellationToken)
        {
            try
            {
                var currentWeek = await scheduleRepository.GetCurrentWeekAsync(cancellationToken);
                
                return currentWeek is not null
                    ? Ok(currentWeek)
                    : NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
