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
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateCandidate([FromBody] CreateCandidateCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> EditCandidate(Guid id, [FromBody] EditCandidateCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> DeleteCandidate(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteCandidateCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("verify-bg/{id:guid}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> VerifyCandidateBg(Guid id, CancellationToken cancellationToken)
        {
            var command = new VerifyCandidateBgCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("/bulk/exel-doc")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateCandidatesWithExelDoc([FromBody] CreateCandidatesWithExelDocCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("/resume-doc")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateCandidateWithResumeDoc([FromBody] CreateCandidateWithResumeDocCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter, HR, Reviewer, Interviewer, Viewer")]
        public async Task<IActionResult> GetCandidate(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetCandidateQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter, HR, Reviewer, Interviewer, Viewer")]
        public async Task<IActionResult> GetCandidates(CancellationToken cancellationToken)
        {
            var query = new GetCandidatesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{candidateId:guid}/verify-doc/{documentId:guid}")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> VerifyCandidateBg(Guid candidateId, Guid documentId, [FromBody] VerifyCandidateDocumentCommand command, CancellationToken cancellationToken)
        {
            command.CandidateId = candidateId;
            command.DocumentId = documentId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{candidateId:guid}/add-document")]
        [Authorize(Roles = "Admin, HR")]
        public async Task<IActionResult> AddCandidateDocument(Guid candidateId, [FromBody] AddCandidateDocumentCommand command, CancellationToken cancellationToken)
        {
            command.Id = candidateId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}