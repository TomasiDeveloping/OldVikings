using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.Database.Entities;
using OldVikings.Api.Dto;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PlayersController(IPlayerRepository playerRepository) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpPut("update/{playerId:guid}")]
        public async Task<ActionResult<PlayerDto>> UpdatePlayer(Guid playerId, UpdatePlayerDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (playerId != dto.Id)
                {
                    return Conflict("Player ID in the URL does not match the ID in the body.");
                }
                return await playerRepository.UpdatePlayerAsync(dto, cancellationToken);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Player>> Create(CreatePlayerDto dto, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await playerRepository.CreateAsync(dto, cancellationToken));
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