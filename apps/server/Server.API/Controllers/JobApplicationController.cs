using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.JobApplications.Commands;
using Server.Application.JobApplications.Queries;
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
        public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> CreateJobApplications([FromBody] CreateJobApplicationsCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("{id:guid}/feedback")]
        public async Task<IActionResult> CreateReviewStageFeedback(Guid id, [FromBody] CreateJobApplicationFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.JobApplicationId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }


        [HttpPut("{applicationId:guid}/feedback/{feedbackId:guid}")]
        public async Task<IActionResult> EditReviewStageFeedback(Guid applicationId, Guid feedbackId, [FromBody] EditJobApplicationFeedbackCommand command, CancellationToken cancellationToken)
        {
            command.JobApplicationId = applicationId;
            command.FeedbackId = feedbackId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("shortlist/{id:guid}")]
        public async Task<IActionResult> ShortlistJobApplication(Guid id, CancellationToken cancellationToken)
        {
            var command = new ShortListApplicationCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteJobApplication(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteJobApplicationCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{applicationId:guid}/feedback/{feedbackId:guid}")]
        public async Task<IActionResult> DeleteReviwStageFeedback(Guid applicationId, Guid feedbackId, CancellationToken cancellationToken)
        {
            var command = new DeleteJobApplicationFeedbackCommand(applicationId, feedbackId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJobApplication(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetJobApplications(CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}