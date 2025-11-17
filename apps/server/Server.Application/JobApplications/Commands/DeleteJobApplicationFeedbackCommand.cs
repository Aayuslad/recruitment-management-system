using MediatR;

using Server.Core.Results;

namespace Server.Application.JobApplications.Commands
{
    public class DeleteJobApplicationFeedbackCommand : IRequest<Result>
    {
        public DeleteJobApplicationFeedbackCommand(Guid jobApplicationId, Guid feedbackId) 
        {
            JobApplicationId = jobApplicationId;
            FeedbackId = feedbackId;
        }

        public Guid JobApplicationId { get; set; }
        public Guid FeedbackId { get; set; }
    }
}
