using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobApplications.Queries.DTOs
{
    public class StatusMoveHistoryDetailDTO
    {
        public Guid Id { get; set; }
        public JobApplicationStatus StatusMovedTo { get; set; }
        public Guid? MovedById { get; set; }
        public string? MovedByName { get; set; }
        public DateTime MovedAt { get; set; }
        public string? Comment { get; set; }
    }
}