using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OldVikings.Api.Classes;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthorizationsController(IOptionsMonitor<R4Settings> optionsMonitor, IJwtService jwtService) : ControllerBase
    {
        private readonly R4Settings _r4Settings = optionsMonitor.CurrentValue;

        [HttpPost("r4-login")]
        public IActionResult R4Login([FromBody] R4LoginRequest request)
        {
            try
            {
                if (request.Password != _r4Settings.Password)
                {
                    return Unauthorized();
                }

                var token = jwtService.GenerateTokenAsync(_r4Settings.Role);

                return Ok(new { Token = token});
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
