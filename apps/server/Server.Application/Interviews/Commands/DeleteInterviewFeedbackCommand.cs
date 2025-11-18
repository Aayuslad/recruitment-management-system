using MediatR;

using Server.Core.Results;

namespace Server.Application.Interviews.Commands
{
    public class DeleteInterviewFeedbackCommand : IRequest<Result>
    {
        public DeleteInterviewFeedbackCommand(Guid interviewId, Guid feedbackid)
        {
            InterviewId = interviewId;
            FeedbackId = feedbackid;
        }

        public Guid InterviewId { get; set; }
        public Guid FeedbackId { get; set; }
    }
}