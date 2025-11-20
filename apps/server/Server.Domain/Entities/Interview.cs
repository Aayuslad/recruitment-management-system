using Server.Core.Entities;
using Server.Core.Primitives;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class Interview : BaseEntity<Guid>, IAggregateRoot
    {
        private Interview() : base(Guid.Empty) { }

        private Interview(
            Guid? id,
            Guid jobApplicationId,
            int roundNumber,
            InterviewType interviewType,
            DateTime? scheduledAt,
            int durationInMinutes,
            string? meetingLink,
            InterviewStatus status,
            IEnumerable<InterviewParticipant> participants
        ) : base(id ?? Guid.NewGuid())
        {
            JobApplicationId = jobApplicationId;
            RoundNumber = roundNumber;
            InterviewType = interviewType;
            ScheduledAt = scheduledAt;
            DurationInMinutes = durationInMinutes;
            MeetingLink = meetingLink;
            Status = status;

            Participants = participants.ToHashSet();
        }

        public Guid JobApplicationId { get; private set; }
        public int RoundNumber { get; private set; }
        public InterviewType InterviewType { get; private set; }
        public DateTime? ScheduledAt { get; private set; }
        public int DurationInMinutes { get; private set; }
        public string? MeetingLink { get; private set; }
        public InterviewStatus Status { get; private set; }
        public JobApplication JobApplication { get; private set; } = null!;
        public ICollection<Feedback> Feedbacks { get; private set; } = new HashSet<Feedback>();
        public ICollection<InterviewParticipant> Participants { get; private set; } = new HashSet<InterviewParticipant>();

        public static Interview Create(
            Guid? id,
            Guid jobApplicationId,
            int roundNumber,
            InterviewType interviewType,
            DateTime? scheduledAt,
            int durationInMinutes,
            string? meetingLink,
            InterviewStatus status,
            IEnumerable<InterviewParticipant> participants
        )
        {
            return new Interview(
                id,
                jobApplicationId,
                roundNumber,
                interviewType,
                scheduledAt,
                durationInMinutes,
                meetingLink,
                status,
                participants
            );
        }

        public void Update(
            int roundNumber,
            InterviewType interviewType,
            DateTime? scheduledAt,
            int durationInMinutes,
            string? meetingLink,
            InterviewStatus status,
            IEnumerable<InterviewParticipant> participants
        )
        {
            RoundNumber = roundNumber;
            InterviewType = interviewType;
            ScheduledAt = scheduledAt;
            DurationInMinutes = durationInMinutes;
            MeetingLink = meetingLink;
            Status = status;

            SyncParticipants(participants);
        }

        public void AddFeedback(Feedback feedback)
        {
            if (feedback is null) return;

            if (!Feedbacks.Any(x => x.Id == feedback.Id))
                Feedbacks.Add(feedback);
        }

        public void UpdateFeedback(
            Guid feedbackId,
            string? comment,
            int rating,
            IEnumerable<SkillFeedback> skillFeedbacks
        )
        {
            var existing = Feedbacks.FirstOrDefault(x => x.Id == feedbackId);
            if (existing is null) return;

            existing.Update(
                comment: comment,
                rating: rating,
                skillFeedbacks: skillFeedbacks
            );
        }

        public void DeleteFeedback(Guid feedbackId)
        {
            var existing = Feedbacks.FirstOrDefault(x => x.Id == feedbackId);
            if (existing is null) return;

            Feedbacks.Remove(existing);
        }

        private void SyncParticipants(IEnumerable<InterviewParticipant> newParticipants)
        {
            if (newParticipants is null) return;

            // remove 
            foreach (var existing in Participants.ToList())
            {
                if (!newParticipants.Any(x => x.Id == existing.Id))
                    Participants.Remove(existing);
            }

            // add 
            foreach (var p in newParticipants)
            {
                if (!Participants.Any(x => x.Id == p.Id))
                    Participants.Add(p);
            }
        }
    }
}