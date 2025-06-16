using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.DataTransferObjects.R4Roles;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class R4RolesController(IR4RoleRepository r4RoleRepository, ILogger<R4RolesController> logger): ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<R4RoleDto>>> GetR4Roles(CancellationToken cancellationToken)
        {
            try
            {
                var r4Roles = await r4RoleRepository.GetR4Roles(cancellationToken);
                return r4Roles.Count > 0 ? Ok(r4Roles) : NoContent();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<R4RoleDto>> UpdateR4Role(Guid id, R4RoleDto r4RoleDto, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
                if (id != r4RoleDto.Id) return BadRequest("ID mismatch");
                var updatedR4Role = await r4RoleRepository.UpdateR4Role(r4RoleDto, cancellationToken);
                return Ok(updatedR4Role);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
