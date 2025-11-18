using Server.Domain.Enums;

namespace Server.Application.Interviews.Queries.DTOs
{
    public class InterviewParticipantDetailDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string ParticipantUserName { get; set; } = null!;
        public InterviewParticipantRole Role { get; set; }
    }
}