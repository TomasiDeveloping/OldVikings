using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using OldVikings.Api.DataTransferObjects.Translation;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TranslationController(ITranslateService translateService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<TranslationResponse>> TranslateText(TranslationRequest request)
        {
            try
            {
                var translatedText = await translateService.Translate(request.Text, request.Language);
                return Ok(translatedText);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
