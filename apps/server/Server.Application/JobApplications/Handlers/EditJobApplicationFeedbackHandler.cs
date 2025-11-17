using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.JobApplications.Handlers
{
    internal class EditJobApplicationFeedbackHandler : IRequestHandler<EditJobApplicationFeedbackCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditJobApplicationFeedbackHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;

        }
        public async Task<Result> Handle(EditJobApplicationFeedbackCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if application exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                return Result.Failure("Job Application does not exists", 409);
            }

            // step 2: check if feedback exists
            var feedback = application.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                return Result.Failure("Feedback does not exist", 404);
            }

            // step 3: authorise (only feedback creator can edit)
            if (feedback.GivenById != Guid.Parse(userIdString))
            {
                return Result.Failure("Unauthorised to feedback edit", 401);
            }

            // step 4: update feedback
            application.UpdateFeedback(
                feedbackId: feedback.Id,
                comment: request.Comment,
                rating: request.Rating,
                skillFeedbacks: request.SkillFeedbacks.Select(
                    selector: x => SkillFeedback.Create(
                            feedbackId: feedback.Id,
                            skillId: x.SkillId,
                            rating: x.Rating,
                            assessedExpYears: x.AssessedExpYears
                        )
                )
            );

            // step 5: persist chagies
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}