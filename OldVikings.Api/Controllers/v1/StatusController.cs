using Microsoft.AspNetCore.Mvc;

namespace OldVikings.Api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
public class StatusController : ControllerBase
{
    [HttpGet]
    public IActionResult Status()
    {
        try
        {
            return Ok("API is running...");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}