using Server.Core.Entities;
using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class PositionBatch : AuditableEntity, IAggregateRoot
    {
        private PositionBatch() : base(Guid.Empty, Guid.Empty) { }
        private PositionBatch(
            Guid createdBy,
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC
        ) : base(Guid.Empty, createdBy)
        {
            DesignationId = designationId;
            Description = description;
            JobLocation = jobLocation;
            MinCTC = minCTC;
            MaxCTC = maxCTC;
        }

        public string? Description { get; private set; }
        public Guid DesignationId { get; private set; } = Guid.Empty;
        public string JobLocation { get; private set; } = string.Empty;
        public float MinCTC { get; private set; } = default;
        public float MaxCTC { get; private set; } = default;
        public Designation Designation { get; private set; } = null!;
        public ICollection<Position> Positions { get; private set; } = new List<Position>();
        public ICollection<PositionBatchReviewers> PositionBatchReviewers { get; private set; } = new List<PositionBatchReviewers>();
        public ICollection<SkillOverRide> SkillOverRides { get; private set; } = new List<SkillOverRide>();

        public static PositionBatch Create(
            Guid createdBy,
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC
        )
        {
            return new PositionBatch(
                createdBy,
                designationId,
                description,
                jobLocation,
                minCTC,
                maxCTC
            );
        }

        public void Update(
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC,
            Guid updatedBy
        )
        {
            DesignationId = designationId;
            Description = description;
            JobLocation = jobLocation;
            MinCTC = minCTC;
            MaxCTC = maxCTC;

            MarkAsUpdated(updatedBy);
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void AddPositions(List<Position> positions)
        {
            foreach (var position in positions)
            {
                Positions.Add(position);
            }
        }

        public void AddReviewers(List<PositionBatchReviewers> reviewers)
        {
            foreach (var reviewer in reviewers)
            {
                if (PositionBatchReviewers.Any(x => x.ReviewerUserId == reviewer.ReviewerUserId))
                    continue;
                PositionBatchReviewers.Add(reviewer);
            }
        }

        public void RemoveReviewers(List<PositionBatchReviewers> reviewers)
        {
            foreach (var reviewer in reviewers)
            {
                if (!PositionBatchReviewers.Contains(reviewer))
                    continue;
                PositionBatchReviewers.Remove(reviewer);
            }
        }

        public void AddSkillOverRides(List<SkillOverRide> skillOverRides)
        {
            foreach (var skill in skillOverRides)
            {
                if (SkillOverRides.Any(x => x.SkillId == skill.SkillId))
                    continue;
                SkillOverRides.Add(skill);
            }
        }

        public void RemoveSkillOverRides(List<SkillOverRide> skillOverRides)
        {
            foreach (var skill in skillOverRides)
            {
                if (!SkillOverRides.Contains(skill))
                    continue;
                SkillOverRides.Remove(skill);
            }
        }
    }
}