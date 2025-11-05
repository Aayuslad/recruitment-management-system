using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient
{
    public class InterviewRoundTemplateSummaryDetailDTO
    {
        public int RoundNumber { get; set; }
        public InterviewType Type { get; set; }
    }
}