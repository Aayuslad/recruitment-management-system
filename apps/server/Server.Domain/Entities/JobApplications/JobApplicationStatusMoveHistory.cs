using Server.Core.Entities;
using Server.Domain.Entities.Users;
using Server.Domain.Enums;

namespace Server.Domain.Entities.JobApplications
{
    public class JobApplicationStatusMoveHistory : BaseEntity<Guid>
    {
        private JobApplicationStatusMoveHistory() : base(Guid.Empty) { }

        private JobApplicationStatusMoveHistory(
           Guid? id,
           Guid jobApplicationId,
           JobApplicationStatus statusMovedTo,
           Guid? movedById,
           string? comment
        ) : base(id ?? Guid.NewGuid())
        {
            JobApplicationId = jobApplicationId;
            StatusMovedTo = statusMovedTo;
            MovedById = movedById;
            MovedAt = DateTime.UtcNow;
            Comment = comment;
        }

        public Guid JobApplicationId { get; private set; }
        public JobApplicationStatus StatusMovedTo { get; private set; }
        public Guid? MovedById { get; private set; }
        public DateTime MovedAt { get; private set; }
        public string? Comment { get; private set; }

        public JobApplication JobApplication { get; private set; } = null!;
        public User? MovedByUser { get; private set; } = null!;

        public static JobApplicationStatusMoveHistory Create(
            Guid? id,
            Guid jobApplicationId,
            JobApplicationStatus statusMovedTo,
            Guid? movedById,
            string? comment
        )
        {
            return new JobApplicationStatusMoveHistory(
                id,
                jobApplicationId,
                statusMovedTo,
                movedById,
                comment
            );
        }
    }
}