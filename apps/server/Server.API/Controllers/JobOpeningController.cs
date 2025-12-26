using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.JobOpenings.Commands;
using Server.Application.Aggregates.JobOpenings.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/job-opening")]
    public class JobOpeningController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOpeningController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateJobOpening([FromBody] CreateJobOpeningCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> EditJobOpening(Guid id, [FromBody] EditJobOpeningCommand command, CancellationToken cancellationToken)
        {
            command.JobOpeningId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobOpening(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteJobOpeningCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetJobOpenings(CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningsForRecruiterQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Viewer, HR, Interviewer, Reviewer")]
        public async Task<IActionResult> GetJobOpening(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningForRecruiterQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}