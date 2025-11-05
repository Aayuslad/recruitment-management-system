using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Commands.DTOs
{
    public class JobOpeningInterviewerDTO
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public InterviewParticipantRole Role { get; set; }
    }
}