using MediatR;

using Server.Application.Interviews.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Interviews.Commands
{
    public class EditInterviewCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public int RoundNumber { get; set; }
        public InterviewType InterviewType { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public int DurationMinutes { get; set; }
        public string? MeetingLink { get; set; }
        public InterviewStatus Status { get; set; }
        public ICollection<InterviewParticipantDTO> Participants { get; set; } =
            new List<InterviewParticipantDTO>();
    }
}