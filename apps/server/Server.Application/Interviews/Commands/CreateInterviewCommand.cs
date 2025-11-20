using MediatR;

using Server.Application.Interviews.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Interviews.Commands
{
    public class CreateInterviewCommand : IRequest<Result>
    {
        public Guid JobApplicationId { get; set; }
        public int RoundNumber { get; set; }
        public InterviewType InterviewType { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int DurationInMinutes { get; set; }
        public string? MeetingLink { get; set; }
        public List<InterviewParticipantDTO> Participants { get; private set; } =
            new List<InterviewParticipantDTO>();
    }
}