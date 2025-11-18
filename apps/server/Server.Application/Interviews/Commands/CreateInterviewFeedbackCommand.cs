using MediatR;

using Server.Application.Interviews.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Interviews.Commands
{
    public class CreateInterviewFeedbackCommand : IRequest<Result>
    {
        public Guid InterviewId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }

        public List<InterviewSkillFeedbackDTO> SkillFeedbacks { get; set; } =
            new List<InterviewSkillFeedbackDTO>();
    }
}