
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Interviews.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.Interviews.Handlers
{
    internal class CreateInterviewHandler : IRequestHandler<CreateInterviewCommand, Result>
    {
        private readonly IInterviewRespository _interviewRespository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateInterviewHandler(IInterviewRespository interviewRespository, IHttpContextAccessor contextAccessor)
        {
            _interviewRespository = interviewRespository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateInterviewCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

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
                    durationMinutes: request.DurationMinutes,
                    meetingLink: request.MeetingLink,
                    status: InterviewStatus.Unscheduled,
                    participants: participants
                );

            // step 2 : persist entity
            await _interviewRespository.AddAsync(interview, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}