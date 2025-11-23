
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.JobApplications.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.JobApplications.Handlers
{
    internal class CreateJobApplicationFeedbackHandler : IRequestHandler<CreateJobApplicationFeedbackCommand, Result>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateJobApplicationFeedbackHandler(IJobApplicationRepository jobApplicationRepository, IHttpContextAccessor contextAccessor)
        {
            _jobApplicationRepository = jobApplicationRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateJobApplicationFeedbackCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: check if exists
            var application = await _jobApplicationRepository.GetByIdAsync(request.JobApplicationId, cancellationToken);
            if (application is null)
            {
                throw new NotFoundExeption("Job Application Not Found.");
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
                    givenById: Guid.Parse(userIdString),
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