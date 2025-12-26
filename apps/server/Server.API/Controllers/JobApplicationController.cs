using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Aggregates.JobApplications.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/job-application")]
    public class JobApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreateJobApplications([FromBody] CreateJobApplicationsCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("{id:guid}/feedback")]
        [Authorize(Roles = "Admin, Recruiter, Reviewer")]
        public async Task<IActionResult> CreateReviewStageFeedback(Guid id, [FromBody] CreateJobApplicationFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.JobApplicationId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }


        [HttpPut("{applicationId:guid}/feedback/{feedbackId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Reviewer")]
        public async Task<IActionResult> EditReviewStageFeedback(Guid applicationId, Guid feedbackId, [FromBody] EditJobApplicationFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.JobApplicationId = applicationId;
            command.FeedbackId = feedbackId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}/move-status")]
        [Authorize(Roles = "Admin, Recruiter, HR")]
        public async Task<IActionResult> MoveStatusJobApplication(Guid id, [FromBody] MoveJobApplicationStatusCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> DeleteJobApplication(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteJobApplicationCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{applicationId:guid}/feedback/{feedbackId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Reviewer")]
        public async Task<IActionResult> DeleteReviwStageFeedback(Guid applicationId, Guid feedbackId, CancellationToken cancellationToken)
        {
            var command = new DeleteJobApplicationFeedbackCommand(applicationId, feedbackId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin, Recruiter, HR, Reviewer, Interviewer, Viewer")]
        public async Task<IActionResult> GetJobApplication(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Recruiter, HR, Reviewer, Interviewer, Viewer")]
        public async Task<IActionResult> GetJobApplications(CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("for-job-opening/{jobOpeningId:guid}")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetJobOpeningApplications(Guid jobOpeningId, CancellationToken cancellationToken)
        {
            var query = new GetJobOpeningApplicationsQuery(jobOpeningId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("to-review")]
        [Authorize(Roles = "Reviewer")]
        public async Task<IActionResult> GetJobApplicationsToReview(CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationsToReviewQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}