using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class JobOpeningInterviewRoundTemplate : BaseEntity<Guid>
    {
        private JobOpeningInterviewRoundTemplate() : base(Guid.Empty) { }

        private JobOpeningInterviewRoundTemplate(
            Guid? id,
            Guid jobOpeningId,
            int roundNumber,
            InterviewType type,
            int durationInMinutes,
            string? description,
            IEnumerable<JobOpeningInterviewPanelRequirement> panelRequirements
        ) : base(id ?? Guid.NewGuid())
        {
            JobOpeningId = jobOpeningId;
            RoundNumber = roundNumber;
            Type = type;
            DurationInMinutes = durationInMinutes;
            Description = description;
            PanelRequirements = panelRequirements?.ToHashSet() ?? [];
        }

        public Guid JobOpeningId { get; private set; }
        public string? Description { get; private set; }
        public int RoundNumber { get; private set; }
        public int DurationInMinutes { get; private set; }
        public InterviewType Type { get; private set; }
        public JobOpening JobOpening { get; private set; } = null!;
        public ICollection<JobOpeningInterviewPanelRequirement> PanelRequirements { get; private set; } =
            new HashSet<JobOpeningInterviewPanelRequirement>();

        public static JobOpeningInterviewRoundTemplate Create(
            Guid? id,
            Guid jobOpeningId,
            int roundNumber,
            InterviewType type,
            int durationInMinutes,
            IEnumerable<JobOpeningInterviewPanelRequirement> panelRequirements,
            string? description = null
        )
        {
            return new JobOpeningInterviewRoundTemplate(
                id,
                jobOpeningId,
                roundNumber,
                type,
                durationInMinutes,
                description,
                panelRequirements
            );
        }

        public void Update(
            int roundNumber,
            InterviewType type,
            int durationInMinutes,
            string? description
        )
        {
            RoundNumber = roundNumber;
            Type = type;
            DurationInMinutes = durationInMinutes;
            Description = description;
        }

        public void SyncPanelRequirements(IEnumerable<JobOpeningInterviewPanelRequirement> newRequirements)
        {
            if (newRequirements is null) return;

            // TODO: rely on Ids here, but first enforce that uniqe constraint in PanelReq entity (mentioned in commet)
            var newIds = newRequirements.Select(x => x.Role).ToHashSet(); // role acts as unique key here

            // remove missing
            var toRemove = PanelRequirements
                .Where(r => !newIds.Contains(r.Role))
                .ToList();

            foreach (var rem in toRemove)
                PanelRequirements.Remove(rem);

            // update existing
            foreach (var existing in PanelRequirements)
            {
                var incoming = newRequirements.FirstOrDefault(x => x.Role == existing.Role);
                if (incoming != null)
                    existing.Update(incoming.Role, incoming.RequiredCount);
            }

            // add new
            foreach (var req in newRequirements)
            {
                if (!PanelRequirements.Any(r => r.Role == req.Role))
                    PanelRequirements.Add(req);
            }
        }
    }
}