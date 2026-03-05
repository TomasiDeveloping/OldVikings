using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.DataTransferObjects.Feedback;
using OldVikings.Api.DataTransferObjects.FeedbackHistory;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbacksController(
        IFeedbackRepository feedbackRepository,
        IFeedbackHistoryRepository feedbackHistoryRepository,
        ILogger<FeedbacksController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> GetFeedbacksByStatus([FromQuery] int[] status,
            CancellationToken cancellationToken)
        {
            try
            {
                var feedbacks = await feedbackRepository.GetFeedbacksByStatusAsync(status, cancellationToken);
                return Ok(feedbacks);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting all feedbacks");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FeedbackDto>> GetFeedback(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var feedback = await feedbackRepository.GetFeedbackAsync(id, cancellationToken);

                return feedback is not null
                    ? Ok(feedback)
                    : NotFound($"Feedback with id {id} not found.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while getting feedback with id {FeedbackId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }

        [HttpGet("{id:guid}/history")]
        public async Task<ActionResult<List<FeedbackHistoryDto>>> GetHistory(Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var feedbackHistory = await feedbackHistoryRepository.GetLast3Async(id, cancellationToken);
                return Ok(feedbackHistory);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while getting feedback history with id {FeedbackId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }

        [HttpGet("retry-discord/{id:guid}")]
        public async Task<IActionResult> RetryDiscordWebhook(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await feedbackRepository.RetryDiscordPostAsync(id, cancellationToken);
                return Ok("Retry process initiated.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrying Discord webhooks");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> CreateFeedback(CreateFeedbackDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (ModelState.IsValid is false) return UnprocessableEntity(ModelState);

                var createdFeedback = await feedbackRepository.CreateFeedbackAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetFeedback), new { id = createdFeedback.Id }, createdFeedback);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating feedback");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }

        [HttpPut("{id:guid}/vote")]
        public async Task<ActionResult<bool>> VoteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await feedbackRepository.VoteAsync(id, cancellationToken);
                return Ok(true);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Feedback with id {id} not found.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while voting for feedback with id {FeedbackId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request.");
            }
        }
    }
}
