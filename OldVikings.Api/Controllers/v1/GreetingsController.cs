﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OldVikings.Api.DataTransferObjects.Greeting;
using OldVikings.Api.Interfaces;

namespace OldVikings.Api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GreetingsController(IGreetingRepository greetingRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<GreetingDto>>> GetGreetings(CancellationToken cancellationToken)
        {
            try
            {
                var greetings = await greetingRepository.GetGreetingsAsync(cancellationToken);
                return greetings.Count > 0 ? Ok(greetings) : NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<GreetingDto>> InsertGreeting(InsertGreetingDto insertGreetingDto,
            CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

                var newGreeting = await greetingRepository.InsertGreetingAsync(insertGreetingDto, cancellationToken);
                return Ok(newGreeting);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}