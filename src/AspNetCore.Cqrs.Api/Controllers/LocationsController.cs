using MediatR;
using AspNetCore.Cqrs.Application.Locations.Queries;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Cqrs.Core.Locations.ReadModels;

namespace AspNetCore.Cqrs.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class LocationsController : ControllerBase
    {
        private readonly ISender _mediator;

        public LocationsController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(LocationReadModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(Guid id)
        {
            var locations = await _mediator.Send(new GetLocationQuery(id));
            return Ok(locations);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<LocationReadModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var locations = await _mediator.Send(new GetLocationsQuery());
            return Ok(locations);
        }
    }
}