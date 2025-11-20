using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class InterviewPanelRequirement : BaseEntity<Guid>
    {
        private InterviewPanelRequirement() : base(Guid.Empty) { }

        private InterviewPanelRequirement(
            Guid? id,
            Guid interviewTemplateId,
            InterviewParticipantRole role,
            int requiredCount
        ) : base(id ?? Guid.NewGuid())
        {
            InterviewTemplateId = interviewTemplateId;
            Role = role;
            RequiredCount = requiredCount;
        }

        public Guid InterviewTemplateId { get; private set; }
        public InterviewParticipantRole Role { get; private set; }
        public int RequiredCount { get; private set; }
        public InterviewRoundTemplate InterviewRoundTemplate { get; private set; } = null!;

        public static InterviewPanelRequirement Create(
            Guid? id,
            Guid interviewTemplateId,
            InterviewParticipantRole role,
            int requiredCount
        )
        {
            return new InterviewPanelRequirement(
                id,
                interviewTemplateId,
                role,
                requiredCount
            );
        }

        public void Update(InterviewParticipantRole role, int requiredCount)
        {
            if (Role != role)
                Role = role;

            if (RequiredCount != requiredCount)
                RequiredCount = requiredCount;
        }
    }
}