using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs
{
    public class InterviewRoundTemplateSummaryDetailDTO
    {
        public int RoundNumber { get; set; }
        public InterviewType Type { get; set; }
    }
}