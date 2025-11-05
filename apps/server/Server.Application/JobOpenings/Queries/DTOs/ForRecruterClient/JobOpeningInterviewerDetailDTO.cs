using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient
{
    public class JobOpeningInterviewerDetailDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public InterviewParticipantRole Role { get; set; }
    }
}