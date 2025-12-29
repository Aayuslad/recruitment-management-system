using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Aggregates.Designations.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/designation")]
    [Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DesignationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDesignation([FromBody] CreateDesignationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditDesignation(Guid id, [FromBody] EditDesignationCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDesignation(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDesignationCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetDesignation(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetDesignationQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetDesignations(CancellationToken cancellationToken)
        {
            var query = new GetDesignationsQuery(null, null, null);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}