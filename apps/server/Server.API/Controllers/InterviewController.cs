using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Aggregates.Interviews.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/interview")]
    [Authorize]
    public class InterviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InterviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInterview([FromBody] CreateInterviewCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditInterview(Guid id, [FromBody] EditInterviewCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteInterview(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteInterviewCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("{id:guid}/feedback")]
        public async Task<IActionResult> CreateInterviewStageFeedback(Guid id, [FromBody] CreateInterviewFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{interviewId:guid}/feedback/{feedbackId:guid}")]
        public async Task<IActionResult> EditInterviewStageFeedback(Guid interviewId, Guid feedbackId, [FromBody] EditInterviewFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = interviewId;
            command.FeedbackId = feedbackId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}/move-status")]
        public async Task<IActionResult> MoveInterviewStatus(Guid id, [FromBody] MoveInterviewStatusCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{interviewId:guid}/feedback/{feedbackId:guid}")]
        public async Task<IActionResult> DeletInterviewStageFeedback(Guid interviewId, Guid feedbackId, CancellationToken cancellationToken)
        {
            var command = new DeleteInterviewFeedbackCommand(interviewId, feedbackId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetInterview(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetInterviewQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> Getinterviews(CancellationToken cancellationToken)
        {
            var query = new GetinterviewsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}