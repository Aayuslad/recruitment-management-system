using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class InterviewParticipant : BaseEntity<Guid>
    {
        private InterviewParticipant() : base(Guid.Empty) { }

        private InterviewParticipant(
            Guid? id,
            Guid interviewId,
            Guid userId,
            InterviewParticipantRole role
        ) : base(id ?? Guid.NewGuid())
        {
            InterviewId = interviewId;
            UserId = userId;
            Role = role;
        }

        public Guid InterviewId { get; private set; }
        public Guid UserId { get; private set; }
        public InterviewParticipantRole Role { get; private set; }
        public Interview Interview { get; private set; } = null!;
        public User User { get; private set; } = null!;

        public static InterviewParticipant Create(
            Guid? id,
            Guid interviewId,
            Guid userId,
            InterviewParticipantRole role
        )
        {
            return new InterviewParticipant(
                id,
                interviewId,
                userId,
                role
            );
        }
    }
}