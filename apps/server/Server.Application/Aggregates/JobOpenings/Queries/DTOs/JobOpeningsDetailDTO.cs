using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Queries.DTOs
{
    public class JobOpeningsDetailDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public JobOpeningType Type { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public List<InterviewRoundTemplateSummaryDetailDTO> InterviewRounds { get; set; } =
            new List<InterviewRoundTemplateSummaryDetailDTO>();
    }
}