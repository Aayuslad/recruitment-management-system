using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class JobOpeningInterviewPanelRequirement : BaseEntity<Guid>
    {
        private JobOpeningInterviewPanelRequirement() : base(Guid.Empty) { }

        private JobOpeningInterviewPanelRequirement(
            Guid? id,
            Guid jobOpeningInterviewTemplateId,
            InterviewParticipantRole role,
            int requiredCount
        ) : base(id ?? Guid.NewGuid())
        {
            JobOpeningInterviewTemplateId = jobOpeningInterviewTemplateId;
            Role = role;
            RequiredCount = requiredCount;
        }

        public Guid JobOpeningInterviewTemplateId { get; private set; }
        public InterviewParticipantRole Role { get; private set; }
        public int RequiredCount { get; private set; }
        public JobOpeningInterviewRoundTemplate InterviewRoundTemplate { get; private set; } = null!;

        public static JobOpeningInterviewPanelRequirement Create(
            Guid? id,
            Guid jobOpeningInterviewTemplateId,
            InterviewParticipantRole role,
            int requiredCount
        )
        {
            return new JobOpeningInterviewPanelRequirement(
                id,
                jobOpeningInterviewTemplateId,
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