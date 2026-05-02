using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.Dto.Train;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TrainsController(ITrainRepository trainRepository) : ControllerBase
    {
        [HttpGet("conductor")]
        public async Task<ActionResult<List<TrainConductorDto>>> GetTrainConductors(CancellationToken cancellationToken)
        {
            try
            {
                var conductors = await trainRepository.GetTrainConductorsAsync(cancellationToken);
                return conductors.Count == 0 ? NoContent() : Ok(conductors);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("vip")]
        public async Task<ActionResult<List<TrainVipDto>>> GetTrainVips(CancellationToken cancellationToken)
        {
            try
            {
                var vips = await trainRepository.GetTrainVipsAsync(cancellationToken);
                return vips.Count == 0 ? NoContent() : Ok(vips);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("vip/{playerId:guid}")]
        public async Task<ActionResult<TrainVipDto>> UpdateTrainVip(Guid playerId, TrainVipDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                if (playerId != dto.PlayerId) return Conflict("VIP ID in URL does not match Player ID in body.");

                var vipDto = await trainRepository.UpdateTrainVip(dto, cancellationToken);
                return Ok(vipDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }

        [HttpPut("conductor/{playerId:guid}")]
        public async Task<ActionResult<TrainConductorDto>> UpdateTrainConductor(Guid playerId, TrainConductorDto dto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                if (playerId != dto.PlayerId) return Conflict("VIP ID in URL does not match Player ID in body.");

                var vipDto = await trainRepository.UpdateTrainConductor(dto, cancellationToken);
                return Ok(vipDto);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

        }
    }
}