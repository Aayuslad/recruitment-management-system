using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient
{
    public class InterviewPanelRequirementDetailDTO
    {
        public Guid? Id { get; set; }
        public InterviewParticipantRole Role { get; set; }
        public int RequiredCount { get; set; }
    }
}