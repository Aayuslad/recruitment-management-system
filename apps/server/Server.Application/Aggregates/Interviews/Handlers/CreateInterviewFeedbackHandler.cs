
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class CreateInterviewFeedbackHandler : IRequestHandler<CreateInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateInterviewFeedbackHandler(IInterviewRepository interviewRepository, IHttpContextAccessor contextAccessor)
        {
            _interviewRepository = interviewRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateInterviewFeedbackCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException($"Interview not found.");
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
            var feedback = Feedback.CreateForInterviewStage(
                    id: feedbackId,
                    jobApplicationId: interview.JobApplicationId,
                    interviewId: interview.Id,
                    givenById: Guid.Parse(userIdString),
                    rating: request.Rating,
                    comment: request.Comment,
                    skillFeedbacks: skillFeedbacks
                );

            // step 3: update root entity
            interview.AddFeedback(feedback);

            // step 4: persist entity
            await _interviewRepository.UpdateAsync(interview, cancellationToken);

            // step 5: return result
            return Result.Success();
        }
    }
}