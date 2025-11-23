
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Aggregates.Interviews.Handlers
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
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch the interviw
            var interview = await _interviewRespository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundExeption("Interview Not Found");
            }

            // step 2: check if feedback exists
            var feedback = interview.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                throw new NotFoundExeption("Feedback Not Found");
            }

            // step 3: authorise (only feedback creator can edit)
            if (feedback.GivenById != Guid.Parse(userIdString))
            {
                throw new ForbiddenExeption("not allowed to edit this feedback");
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