using MediatR;

using Server.Application.JobApplications.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.JobApplications.Commands
{
    public class CreateJobApplicationFeedbackCommand : IRequest<Result>
    {
        public Guid JobApplicationId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }

        public List<SkillFeedbackDTO> SkillFeedbacks { get; set; } =
            new List<SkillFeedbackDTO>();
    }
}
