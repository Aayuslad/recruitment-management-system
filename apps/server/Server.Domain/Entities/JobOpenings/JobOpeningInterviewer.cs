using Server.Core.Entities;
using Server.Domain.Entities.Users;
using Server.Domain.Enums;

namespace Server.Domain.Entities.JobOpenings
{
    public class JobOpeningInterviewer : BaseEntity<Guid>
    {
        private JobOpeningInterviewer() : base(Guid.Empty) { }

        private JobOpeningInterviewer(
            Guid? id,
            Guid jobOpeningId,
            Guid userId,
            InterviewParticipantRole role
        ) : base(id ?? Guid.NewGuid())
        {
            JobOpeningId = jobOpeningId;
            UserId = userId;
            Role = role;
        }

        public Guid JobOpeningId { get; private set; }
        public Guid UserId { get; private set; }
        public InterviewParticipantRole Role { get; private set; }
        public JobOpening JobOpening { get; private set; } = null!;
        public User InterviewerUser { get; private set; } = null!;

        public static JobOpeningInterviewer Create(
            Guid? id,
            Guid jobOpeningId,
            Guid userId,
            InterviewParticipantRole role
        )
        {
            // TODO: add logic for (JobOpeningId + UserId + Role) must be unique
            // at db level it is added

            return new JobOpeningInterviewer(
                id,
                jobOpeningId,
                userId,
                role
            );
        }

        public void Update(InterviewParticipantRole role)
        {
            if (Role != role)
                Role = role;
        }
    }
}