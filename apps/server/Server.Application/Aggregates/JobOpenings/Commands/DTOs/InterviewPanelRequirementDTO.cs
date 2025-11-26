using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Commands.DTOs
{
    public class InterviewPanelRequirementDTO
    {
        public Guid? Id { get; set; }
        public Guid? JobOpeningInterviewTemplateId { get; set; }
        public InterviewParticipantRole Role { get; set; }
        public int RequirementCount { get; set; }
    }
}