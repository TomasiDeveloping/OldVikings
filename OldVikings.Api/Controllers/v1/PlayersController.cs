using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PlayersController(IPlayerRepository playerRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<PlayerDto>>> GetPlayers(CancellationToken cancellationToken)
        {
            try
            {
                var players = await playerRepository.GetPlayersAsync(cancellationToken);
                return players.Count == 0 ? NoContent() : Ok(players);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut("{playerId:guid}")]
        public async Task<ActionResult<bool>> SetPlayerActive(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                await playerRepository.SetPlayerActiveAsync(playerId, cancellationToken);
                return Ok(true);
            }
            catch (ApplicationException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Player>> Create([FromBody] string playerName,
            CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await playerRepository.CreateAsync(playerName, cancellationToken));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{playerId:guid}")]
        public async Task<IActionResult> DeletePlayer(Guid playerId, CancellationToken cancellationToken)
        {
            try
            {
                await playerRepository.DeletePlayerAsync(playerId, cancellationToken);
                return NoContent();
            }
            catch (ApplicationException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}