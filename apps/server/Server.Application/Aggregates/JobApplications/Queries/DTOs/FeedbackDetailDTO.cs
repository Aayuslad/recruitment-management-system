using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Queries.DTOs
{
    public class FeedbackDetailDTO
    {
        public Guid Id { get; set; }
        public Guid GivenById { get; set; }
        public string GivenByName { get; set; } = null!;
        public FeedbackStage Stage { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public ICollection<SkillFeedbackDetailDTO> SkillFeedbacks { get; set; } =
            new List<SkillFeedbackDetailDTO>();
    }
}