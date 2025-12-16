using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Queries.DTOs
{
    public class JobOpeningDetailDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public JobOpeningType Type { get; set; }
        public Guid PositionBatchId { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public string JobLocation { get; set; } = null!;
        public float MinCTC { get; set; }
        public float MaxCTC { get; set; }
        public int PositionsCount { get; set; }
        public int ClosedPositionsCount { get; set; }
        public List<SkillDetailDTO> Skills { get; set; } = new List<SkillDetailDTO>();
        public List<SkillOverRideDetailDTO> SkillOverRides { get; set; } =
            new List<SkillOverRideDetailDTO>();
        public List<JobOpeningInterviewerDetailDTO> Interviewers { get; set; } =
            new List<JobOpeningInterviewerDetailDTO>();
        public List<InterviewRoundTemplateDetailDTO> InterviewRounds { get; set; } =
            new List<InterviewRoundTemplateDetailDTO>();
    }
}