
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobApplications.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Aggregates.JobApplications.Handlers
{
    internal class CreateJobApplicationFeedbackHandler : IRequestHandler<CreateJobApplicationFeedbackCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IUserContext _userContext;

        public CreateJobApplicationFeedbackHandler(IJobApplicationRepository jobApplicationRepository, IUserContext userContext)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateJobApplicationFeedbackCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                throw new NotFoundException("Job Application Not Found.");
            }

            // step 2: create and add feedback entities
            var feedbackId = Guid.NewGuid();
            var skillFeedbacks = new List<SkillFeedback>();

            // create list of skill feedbacks
            foreach (var skillFeedbackDto in request.SkillFeedbacks)
            {
                var skillFeedback = SkillFeedback.Create(
                        feedbackId: feedbackId,
                        skillId: skillFeedbackDto.SkillId,
                        rating: skillFeedbackDto.Rating,
                        assessedExpYears: skillFeedbackDto.AssessedExpYears
                    );
                skillFeedbacks.Add(skillFeedback);
            }

            // create the feedback entity
            var feedback = Feedback.CreateForReviewStage(
                    id: feedbackId,
                    jobApplicationId: application.Id,
                    givenById: _userContext.UserId,
                    rating: request.Rating,
                    comment: request.Comment,
                    skillFeedbacks: skillFeedbacks
                );

            // step 3: update root entity
            application.AddFeedback(feedback);

            // step 4: persist entity
            await _jobApplicationRepository.UpdateAsync(application, cancellationToken);

            // step 5: return result
            return Result.Success();
        }
    }
}