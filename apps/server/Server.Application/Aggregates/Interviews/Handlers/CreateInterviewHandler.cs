
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Core.Results;
using Server.Domain.Entities.Interviews;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class CreateInterviewHandler : IRequestHandler<CreateInterviewCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserContext _userContext;

        public CreateInterviewHandler(IInterviewRepository interviewRepository, IUserContext userContext)
        {
            _interviewRepository = interviewRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
        {
            // step 1: create entity
            var newInterviewId = Guid.NewGuid();

            // create participant entity list
            var participants = request.Participants.Select(
                    selector: x => InterviewParticipant.Create(
                            id: null,
                            interviewId: newInterviewId,
                            userId: x.UserId,
                            role: x.Role
                        )
                );

            // create root entity
            var interview = Interview.Create(
                    id: newInterviewId,
                    jobApplicationId: request.JobApplicationId,
                    roundNumber: request.RoundNumber,
                    interviewType: request.InterviewType,
                    scheduledAt: request.ScheduledAt,
                    durationInMinutes: request.DurationInMinutes,
                    meetingLink: request.MeetingLink,
                    status: InterviewStatus.NotScheduled,
                    participants: participants
                );

            // step 2 : persist entity
            await _interviewRepository.AddAsync(interview, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}