using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class EditJobApplicationFeedbackHandler : IRequestHandler<EditJobApplicationFeedbackCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserContext _userContext;

        public EditJobApplicationFeedbackHandler(IJobApplicationRepository jobApplicationRepository, IUserContext userContext)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _userContext = userContext;

        }
        public async Task<Result> Handle(EditJobApplicationFeedbackCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if application exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                throw new NotFoundException("Job Application Not Found.");
            }

            // step 2: check if feedback exists
            var feedback = application.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                throw new NotFoundException("Feedback Not Found.");
            }

            // step 3: authorise (only feedback creator can edit)
            if (feedback.GivenById != _userContext.UserId)
            {
                throw new ForbiddenException("not allowed to edit this feedback.");
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