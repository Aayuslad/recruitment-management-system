using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;

namespace Server.Application.JobApplications.Handlers
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
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                return Result.Failure("Job Application does not exists", 409);
            }

            // step 2: find feedback to delete
            var feedback = application.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                return Result.Failure("Feedback does not exist", 404);
            }

            // TODO: allow admin to do this when RABC applied
            // step 3: authorise (only feedback creator can delete)
            if (feedback.GivenById != Guid.Parse(userIdString))
            {
                return Result.Failure("Unauthorised to feedback delete", 401);
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