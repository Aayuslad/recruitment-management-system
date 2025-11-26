using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Commands.DTOs
{
    public class InterviewParticipantDTO
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public InterviewParticipantRole Role { get; set; }
    }
}