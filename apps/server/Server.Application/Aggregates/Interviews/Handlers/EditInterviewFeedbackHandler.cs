
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class EditInterviewFeedbackHandler : IRequestHandler<EditInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserContext _userContext;

        public EditInterviewFeedbackHandler(IInterviewRepository interviewRepository, IUserContext userContext)
        {
            _interviewRepository = interviewRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditInterviewFeedbackCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException("Interview Not Found");
            }

            // step 2: check if feedback exists
            var feedback = interview.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                throw new NotFoundException("Feedback Not Found");
            }

            // step 3: authorise (only feedback creator can edit)
            if (feedback.GivenById != _userContext.UserId)
            {
                throw new ForbiddenException("not allowed to edit this feedback");
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
            await _interviewRepository.UpdateAsync(interview, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}