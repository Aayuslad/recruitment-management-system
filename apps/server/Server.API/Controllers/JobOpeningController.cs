using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.JobOpenings.Commands;
using Server.Application.JobOpenings.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/job-opening")]
    public class JobOpeningController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobOpeningController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobOpening([FromBody] CreateJobOpeningCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditJobOpening(Guid id, [FromBody] EditJobOpeningCommand command, CancellationToken cancellationToken)
        {
            command.JobOpeningId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteJobOpening(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteJobOpeningCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobOpeningsForRecruiter(CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningsForRecruiterQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJobOpeningForRecruiter(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningForRecruiterQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("for-candidate")]
        public async Task<IActionResult> GetJobOpeningsForCandidate(CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningsForCandidateQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("for-candidate/{id:guid}")]
        public async Task<IActionResult> GetJobOpeningForCandidate(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningForCandiateQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}