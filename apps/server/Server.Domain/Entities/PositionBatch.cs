using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;

namespace Server.Domain.Entities
{
    public class PositionBatch : AuditableEntity, IAggregateRoot
    {
        private PositionBatch() : base(Guid.Empty, Guid.Empty) { }

        private PositionBatch(
            Guid? id,
            Guid createdBy,
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC,
            IEnumerable<Position> positions,
            IEnumerable<PositionBatchReviewer> reviewers,
            IEnumerable<SkillOverRide> overRides
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            DesignationId = designationId;
            Description = description;
            JobLocation = jobLocation;
            MinCTC = minCTC;
            MaxCTC = maxCTC;
            Positions = positions.ToHashSet();
            Reviewers = reviewers.ToHashSet();
            SkillOverRides = overRides.ToHashSet();
        }

        public string? Description { get; private set; }
        public Guid DesignationId { get; private set; } = Guid.Empty;
        public string JobLocation { get; private set; } = string.Empty;
        public float MinCTC { get; private set; } = default;
        public float MaxCTC { get; private set; } = default;
        public Designation Designation { get; private set; } = null!;
        public ICollection<Position> Positions { get; private set; } =
            new HashSet<Position>();
        public ICollection<PositionBatchReviewer> Reviewers { get; private set; } =
            new HashSet<PositionBatchReviewer>();
        public ICollection<SkillOverRide> SkillOverRides { get; private set; } =
            new HashSet<SkillOverRide>();

        public static PositionBatch Create(
            Guid? id,
            Guid createdBy,
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC,
            IEnumerable<Position> positions,
            IEnumerable<PositionBatchReviewer> reviewers,
            IEnumerable<SkillOverRide> overRides
        )
        {
            return new PositionBatch(
                id,
                createdBy,
                designationId,
                description,
                jobLocation,
                minCTC,
                maxCTC,
                positions,
                reviewers,
                overRides
            );
        }

        public void Update(
            Guid designationId,
            string? description,
            string jobLocation,
            float minCTC,
            float maxCTC,
            IEnumerable<PositionBatchReviewer> newReviewers,
            IEnumerable<SkillOverRide> newOverRides,
            Guid updatedBy
        )
        {
            DesignationId = designationId;
            Description = description;
            JobLocation = jobLocation;
            MinCTC = minCTC;
            MaxCTC = maxCTC;

            SyncReviewers(newReviewers);
            SyncSkillOverRides(newOverRides);

            MarkAsUpdated(updatedBy);
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        private void SyncReviewers(IEnumerable<PositionBatchReviewer> newReviewers)
        {
            if (newReviewers is null) return;

            // remove removed ones
            foreach (var existing in Reviewers.ToList())
            {
                if (!newReviewers.Any(x => x.ReviewerId == existing.ReviewerId))
                    Reviewers.Remove(existing);
            }

            // no update section, join table has no updatable data

            // add added ones
            foreach (var reviewer in newReviewers)
            {
                if (!Reviewers.Any(x => x.ReviewerId == reviewer.ReviewerId))
                    Reviewers.Add(reviewer);
            }
        }

        private void SyncSkillOverRides(IEnumerable<SkillOverRide> newOverRides)
        {
            if (newOverRides is null) return;

            // remove removed ones
            foreach (var overRide in SkillOverRides.ToList())
            {
                if (!newOverRides.Any(x => x.Id == overRide.Id))
                    SkillOverRides.Remove(overRide);
            }

            // update existing
            foreach (var overRide in newOverRides)
            {
                var toUpdate = SkillOverRides.FirstOrDefault(x => x.Id == overRide.Id);
                toUpdate?.Update(
                        overRide.Comments,
                        overRide.MinExperienceYears,
                        overRide.Type,
                        overRide.ActionType
                    );
            }

            // add added ones
            foreach (var overRide in newOverRides)
            {
                if (!SkillOverRides.Any(x => x.Id == overRide.Id))
                    SkillOverRides.Add(overRide);
            }
        }
    }
}