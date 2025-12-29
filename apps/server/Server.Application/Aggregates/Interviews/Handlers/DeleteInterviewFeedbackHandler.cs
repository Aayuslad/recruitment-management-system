
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

using static System.Net.Mime.MediaTypeNames;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class DeleteInterviewFeedbackHandler : IRequestHandler<DeleteInterviewFeedbackCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteInterviewFeedbackHandler(IInterviewRepository interviewRepository, IHttpContextAccessor contextAccessor)
        {
            _interviewRepository = interviewRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteInterviewFeedbackCommand request, CancellationToken cancellationToken)
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
            if (feedback.GivenById != Guid.Parse(userIdString))
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