using Server.Core.Entities;
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

        public void SyncInterviewers(IEnumerable<JobOpeningInterviewer> newInterviewers)
        {
            if (newInterviewers is null) return;

            var newIds = newInterviewers.Select(x => x.Id).ToHashSet();

            // remove missing ones
            var toRemove = JobOpeningInterviewers
                .Where(x => !newIds.Contains(x.Id))
                .ToList();

            foreach (var rem in toRemove)
                JobOpeningInterviewers.Remove(rem);

            // update existing
            foreach (var existing in JobOpeningInterviewers)
            {
                var toUpdate = newInterviewers.FirstOrDefault(x => x.Id == existing.Id);
                if (toUpdate != null)
                    existing.Update(toUpdate.Role);
            }

            // add new ones
            foreach (var interviewer in newInterviewers)
            {
                if (!JobOpeningInterviewers.Any(x => x.Id == interviewer.Id))
                    JobOpeningInterviewers.Add(interviewer);
            }
        }

        public void SyncInterviewRounds(IEnumerable<JobOpeningInterviewRoundTemplate> newRounds)
        {
            if (newRounds is null) return;

            var newIds = newRounds.Select(x => x.Id).ToHashSet();

            // remove missing
            var toRemove = InterviewRounds
                .Where(r => !newIds.Contains(r.Id))
                .ToList();

            foreach (var rem in toRemove)
                InterviewRounds.Remove(rem);

            // update existing
            foreach (var existing in InterviewRounds)
            {
                var toUpdate = newRounds.FirstOrDefault(x => x.Id == existing.Id);
                if (toUpdate != null)
                {
                    existing.Update(
                        toUpdate.RoundNumber,
                        toUpdate.Type,
                        toUpdate.DurationInMinutes,
                        toUpdate.Description
                    );

                    // sync panel requirements of each round
                    existing.SyncPanelRequirements(toUpdate.PanelRequirements);
                }
            }

            // add new
            foreach (var round in newRounds)
            {
                if (!InterviewRounds.Any(r => r.Id == round.Id))
                    InterviewRounds.Add(round);
            }
        }

        public void SyncSkillOverRides(IEnumerable<SkillOverRide> newSkills)
        {
            if (newSkills is null) return;

            var newIds = newSkills.Select(x => x.SkillId).ToHashSet();

            // remove missing
            var toRemove = SkillOverRides
                .Where(s => !newIds.Contains(s.SkillId))
                .ToList();

            foreach (var x in toRemove)
                SkillOverRides.Remove(x);

            // update existing
            foreach (var existing in SkillOverRides)
            {
                var incoming = newSkills.FirstOrDefault(x => x.SkillId == existing.SkillId);
                if (incoming != null)
                    existing.Update(
                        incoming.Comments,
                        incoming.MinExperienceYears,
                        incoming.Type,
                        incoming.ActionType
                    );
            }

            // add new
            foreach (var skill in newSkills)
            {
                if (!SkillOverRides.Any(s => s.SkillId == skill.SkillId))
                    SkillOverRides.Add(skill);
            }
        }
    }
}