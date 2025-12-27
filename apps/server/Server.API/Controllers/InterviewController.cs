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
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateInterview([FromBody] CreateInterviewCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> EditInterview(Guid id, [FromBody] EditInterviewCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> DeleteInterview(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteInterviewCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("{id:guid}/feedback")]
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        public async Task<IActionResult> CreateInterviewStageFeedback(Guid id, [FromBody] CreateInterviewFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{interviewId:guid}/feedback/{feedbackId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        public async Task<IActionResult> EditInterviewStageFeedback(Guid interviewId, Guid feedbackId, [FromBody] EditInterviewFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = interviewId;
            command.FeedbackId = feedbackId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}/move-status")]
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        public async Task<IActionResult> MoveInterviewStatus(Guid id, [FromBody] MoveInterviewStatusCommand command, CancellationToken cancellationToken)
        {
            command.InterviewId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{interviewId:guid}/feedback/{feedbackId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Interviewer")]
        public async Task<IActionResult> DeletInterviewStageFeedback(Guid interviewId, Guid feedbackId, CancellationToken cancellationToken)
        {
            var command = new DeleteInterviewFeedbackCommand(interviewId, feedbackId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter, HR, Interviewer, Viewer")]
        public async Task<IActionResult> GetInterview(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetInterviewQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "Interviewer, Admin, Recruiter, HR")]
        public async Task<IActionResult> GetAssignedInterviews(CancellationToken cancellationToken)
        {
            var query = new GetAssignedInterviewsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("job-application/{jobApplicationId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, HR, Viewer, Interviewer, Reviewer")]
        public async Task<IActionResult> GetJobApplicationInterviews(Guid jobApplicationId, CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationInterviewsQuery(jobApplicationId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}