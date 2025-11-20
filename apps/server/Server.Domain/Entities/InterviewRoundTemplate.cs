using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class InterviewRoundTemplate : BaseEntity<Guid>
    {
        private InterviewRoundTemplate() : base(Guid.Empty) { }

        private InterviewRoundTemplate(
            Guid? id,
            Guid jobOpeningId,
            int roundNumber,
            InterviewType type,
            int durationInMinutes,
            string? description,
            IEnumerable<InterviewPanelRequirement> panelRequirements
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
        public ICollection<InterviewPanelRequirement> PanelRequirements { get; private set; } =
            new HashSet<InterviewPanelRequirement>();

        public static InterviewRoundTemplate Create(
            Guid? id,
            Guid jobOpeningId,
            int roundNumber,
            InterviewType type,
            int durationInMinutes,
            IEnumerable<InterviewPanelRequirement> panelRequirements,
            string? description = null
        )
        {
            return new InterviewRoundTemplate(
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

        public void SyncPanelRequirements(IEnumerable<InterviewPanelRequirement> newRequirements)
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