using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class DeleteJobApplicationFeedbackHandler : IRequestHandler<DeleteJobApplicationFeedbackCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteJobApplicationFeedbackHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteJobApplicationFeedbackCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                throw new NotFoundException("Job Application Not Found.");
            }

            // step 2: find feedback to delete
            var feedback = application.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                throw new NotFoundException("Feedback Not Found.");
            }

            // TODO: allow admin to do this when RABC applied
            // step 3: authorise (only feedback creator can delete)
            if (feedback.GivenById != Guid.Parse(userIdString))
            {
                throw new ForbiddenException("not allowed to delete this feedback.");
            }

            // step 4: delete feedback
            application.DeleteFeedback(feedback.Id);

            // step 5: persist entity
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}