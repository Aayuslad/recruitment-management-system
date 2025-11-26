using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Queries.DTOs
{
    public class InterviewRoundTemplateDetailDTO
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public int RoundNumber { get; set; }
        public int DurationInMinutes { get; set; }
        public InterviewType Type { get; set; }
        public List<InterviewPanelRequirementDetailDTO> Requirements { get; set; } =
            new List<InterviewPanelRequirementDetailDTO>();
    }
}