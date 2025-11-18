
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Interviews.Commands;
using Server.Core.Results;

using static System.Net.Mime.MediaTypeNames;

namespace Server.Application.Interviews.Handlers
{
    internal class DeleteInterviewFeedbackHandler : IRequestHandler<DeleteInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRespository _interviewRespository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteInterviewFeedbackHandler(IInterviewRespository interviewRespository, IHttpContextAccessor contextAccessor)
        {
            _interviewRespository = interviewRespository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteInterviewFeedbackCommand request, CancellationToken cancellationToken)
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

            // step 2: find feedback to delete
            var feedback = interview.Feedbacks.FirstOrDefault(x => x.Id == request.FeedbackId);
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
            interview.DeleteFeedback(feedback.Id);

            // step 5: persist entity
            await _interviewRespository.UpdateAsync(interview, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}