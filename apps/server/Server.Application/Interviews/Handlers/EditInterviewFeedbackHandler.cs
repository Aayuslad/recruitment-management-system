
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Interviews.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Interviews.Handlers
{
    internal class EditInterviewFeedbackHandler : IRequestHandler<EditInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRespository _interviewRespository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditInterviewFeedbackHandler(IInterviewRespository interviewRespository, IHttpContextAccessor contextAccessor)
        {
            _interviewRespository = interviewRespository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(EditInterviewFeedbackCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the interviw
            var interview = await _interviewRespository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                return Result.Failure("interview does not exist", 404);
            }

            // step 2: check if feedback exists
            var feedback = interview.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
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
            interview.UpdateFeedback(
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
            await _interviewRespository.UpdateAsync(interview, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}