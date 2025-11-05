using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Commands.DTOs
{
    public class InterviewRoundTemplateDTO
    {
        public Guid? Id { get; set; }
        public string? Description { get; set; }
        public int RoundNumber { get; set; }
        public int DurationInMinutes { get; set; }
        public InterviewType Type { get; set; }
        public List<InterviewPanelRequirementDTO> Requirements { get; set; } = new List<InterviewPanelRequirementDTO>();
    }
}