using MediatR;
using AspNetCore.Cqrs.Application.Weather.Commands;
using AspNetCore.Cqrs.Application.Weather.Queries;
using AspNetCore.Cqrs.Api.Infrastructure.ActionResults;
using AspNetCore.Cqrs.Application.Weather.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Cqrs.Core.Weather.ReadModels;

namespace AspNetCore.Cqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class WeatherForecastsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherForecastsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<WeatherForecastReadModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var forecast = await _mediator.Send(new GetWeatherForecastQuery(id));
            return Ok(forecast);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<WeatherForecastReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] Guid? locationId)
        {
            var forecasts = await _mediator.Send(new GetWeatherForecastsQuery(locationId));
            return Ok(forecasts);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreatedResultEnvelope), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Post([FromBody] WeatherForecastCreateDto forecast)
        {
            var id = Guid.NewGuid();
            await _mediator.Send(new CreateWeatherForecastCommand(id, forecast.TemperatureC, forecast.Date, forecast.Summary, forecast.LocationId));
            return CreatedAtAction(nameof(Get), new { id }, new CreatedResultEnvelope(id));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(Guid id, [FromBody] WeatherForecastUpdateDto forecast)
        {
            await _mediator.Send(new UpdateWeatherForecastCommand(id, forecast.Date));
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Envelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteWeatherForecastCommand(id));
            return NoContent();
        }
    }
}