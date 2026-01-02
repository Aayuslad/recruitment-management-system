
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class DeleteInterviewFeedbackHandler : IRequestHandler<DeleteInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserContext _userContext;

        public DeleteInterviewFeedbackHandler(IInterviewRepository interviewRepository, IUserContext userContext)
        {
            _interviewRepository = interviewRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteInterviewFeedbackCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.InterviewId, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException("Interview Not Found");
            }

            // step 2: find feedback to delete
            var feedback = interview.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
            if (feedback is null)
            {
                throw new NotFoundException("Feedback Not Found");
            }

            // TODO: allow admin to do this when RABC applied
            // step 3: authorise (only feedback creator can delete)
            if (feedback.GivenById != _userContext.UserId)
            {
                throw new ForbiddenException("Not allowed to delete this feedback");
            }

            // step 4: delete feedback
            interview.DeleteFeedback(feedback.Id);

            // step 5: persist entity
            await _interviewRepository.UpdateAsync(interview, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}