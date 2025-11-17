using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Queries.DTOs.ForRecruterClient
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
        public List<SkillDetailDTO> Skills { get; set; } = new List<SkillDetailDTO>();
        public List<SkillOverRideDetailDTO> SkillOverRides { get; set; } =
            new List<SkillOverRideDetailDTO>();
        public List<JobOpeningInterviewerDetailDTO> Interviewers { get; set; } =
            new List<JobOpeningInterviewerDetailDTO>();
        public List<InterviewRoundTemplateDetailDTO> InterviewRounds { get; set; } =
            new List<InterviewRoundTemplateDetailDTO>();
    }
}