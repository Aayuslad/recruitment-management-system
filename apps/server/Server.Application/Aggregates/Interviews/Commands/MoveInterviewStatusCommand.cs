using MediatR;

using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Commands
{
    public class MoveInterviewStatusCommand : IRequest<Result>
    {
        public Guid InterviewId { get; set; }
        public InterviewStatus MoveTo { get; set; }
        public string? MeetingLink { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }
}