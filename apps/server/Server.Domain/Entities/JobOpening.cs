using Server.Core.Primitives;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class JobOpening : AuditableEntity, IAggregateRoot
    {
        private JobOpening() : base(Guid.Empty, Guid.Empty) { }

        private JobOpening(
            Guid? id,
            Guid createdBy,
            Guid positionBatchId,
            string title,
            JobOpeningType type,
            string? description,
            IEnumerable<JobOpeningInterviewer> jobOpeningInterviewers,
            IEnumerable<JobOpeningInterviewRoundTemplate> interviewRounds,
            IEnumerable<SkillOverRide> skillOverRides
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            Title = title;
            Description = description;
            Type = type;
            PositionBatchId = positionBatchId;
            JobOpeningInterviewers = jobOpeningInterviewers.ToHashSet();
            InterviewRounds = interviewRounds.ToHashSet();
            SkillOverRides = skillOverRides.ToHashSet();
        }

        public string Title { get; private set; } = null!;
        public string? Description { get; private set; }
        public JobOpeningType Type { get; private set; }
        public Guid PositionBatchId { get; private set; }
        public PositionBatch PositionBatch { get; private set; } = null!;
        public ICollection<JobOpeningInterviewer> JobOpeningInterviewers { get; private set; } =
            new HashSet<JobOpeningInterviewer>();
        public ICollection<JobOpeningInterviewRoundTemplate> InterviewRounds { get; private set; } =
            new HashSet<JobOpeningInterviewRoundTemplate>();
        public ICollection<SkillOverRide> SkillOverRides { get; private set; } =
            new HashSet<SkillOverRide>();

        public static JobOpening Create(
            Guid? id,
            Guid createdBy,
            Guid positionBatchId,
            string title,
            JobOpeningType type,
            string? description,
            IEnumerable<JobOpeningInterviewer> jobOpeningInterviewers,
            IEnumerable<JobOpeningInterviewRoundTemplate> interviewRounds,
            IEnumerable<SkillOverRide> skillOverRides
        )
        {
            return new JobOpening(
                id,
                createdBy,
                positionBatchId,
                title,
                type,
                description,
                jobOpeningInterviewers,
                interviewRounds,
                skillOverRides
            );
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void Update(
            Guid updatedBy,
            Guid positionBatchId,
            string title,
            JobOpeningType type,
            string? description,
            IEnumerable<JobOpeningInterviewer> jobOpeningInterviewers,
            IEnumerable<JobOpeningInterviewRoundTemplate> interviewRounds,
            IEnumerable<SkillOverRide> skillOverRides
        )
        {
            Title = title;
            Description = description;
            Type = type;
            PositionBatchId = positionBatchId;

            SyncInterviewers(jobOpeningInterviewers);
            SyncInterviewRounds(interviewRounds);
            SyncSkillOverRides(skillOverRides);

            MarkAsUpdated(updatedBy);
        }

        private void SyncInterviewers(IEnumerable<JobOpeningInterviewer> newInterviewers)
        {
            if (newInterviewers is null)
                return;

            // remove missing ones
            foreach (var existing in JobOpeningInterviewers.ToList())
            {
                if (!newInterviewers.Any(x => x.Id == existing.Id))
                    JobOpeningInterviewers.Remove(existing);
            }

            // update existing
            foreach (var interviewer in newInterviewers)
            {
                var toUpdate = JobOpeningInterviewers.FirstOrDefault(x => x.Id == interviewer.Id);
                toUpdate?.Update(interviewer.Role);
            }

            // add new ones
            foreach (var interviewer in newInterviewers)
            {
                if (!JobOpeningInterviewers.Any(x => x.Id == interviewer.Id))
                    JobOpeningInterviewers.Add(interviewer);
            }
        }

        private void SyncInterviewRounds(IEnumerable<JobOpeningInterviewRoundTemplate> newRounds)
        {
            if (newRounds is null)
                return;

            // remove missing ones
            foreach (var existing in InterviewRounds.ToList())
            {
                if (!newRounds.Any(x => x.Id == existing.Id))
                    InterviewRounds.Remove(existing);
            }

            // update existing
            foreach (var round in newRounds)
            {
                var toUpdate = InterviewRounds.FirstOrDefault(x => x.Id == round.Id);
                if (toUpdate is not null)
                {
                    toUpdate.Update(
                        round.RoundNumber,
                        round.Type,
                        round.DurationInMinutes,
                        round.Description
                    );

                    // sync panel requirements inside the round
                    toUpdate.SyncPanelRequirements(round.PanelRequirements);
                }
            }

            // add new ones
            foreach (var round in newRounds)
            {
                if (!InterviewRounds.Any(x => x.Id == round.Id))
                    InterviewRounds.Add(round);
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