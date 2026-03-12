using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.Dto;
using OldVikings.Api.Interfaces;


namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchedulesController(IScheduleRepository scheduleRepository) : ControllerBase
    {
        [HttpGet("history")]
        public async Task<ActionResult<List<WeeklyScheduleDto>>> GetHistory(
            CancellationToken cancellationToken,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? playerName = null,
            [FromQuery] int? year = null
            )
        {
            try
            {
                var history = await scheduleRepository.GetHistoryAsync(page, pageSize, playerName, year, cancellationToken);

                return Ok(history);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

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

        [HttpGet("next-week")]
        public async Task<ActionResult<WeeklyScheduleDto>> GetNextWeek([FromQuery]DateOnly date, CancellationToken cancellationToken)
        {
            try
            {
                var currentWeek = await scheduleRepository.GetWeekAfterNextAsync(date,cancellationToken);

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
