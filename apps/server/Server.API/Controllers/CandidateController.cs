using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Aggregates.Candidates.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/candidate")]
    public class CandidateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCandidate(Guid id, [FromBody] EditCandidateCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCandidate(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCandidateCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("verify-bg/{id:guid}")]
        public async Task<IActionResult> VerifyCandidateBg(Guid id, CancellationToken cancellationToken)
        {
            var command = new VerifyCandidateBgCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("/bulk/exel-doc")]
        public async Task<IActionResult> CreateCandidatesWithExelDoc([FromBody] CreateCandidatesWithExelDocCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("/resume-doc")]
        public async Task<IActionResult> CreateCandidateWithResumeDoc([FromBody] CreateCandidateWithResumeDocCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCandidate(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCandidateQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetCandidates(CancellationToken cancellationToken)
        {
            var query = new GetCandidatesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}