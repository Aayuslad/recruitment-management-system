
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Interviews.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Interviews.Handlers
{
    internal class EditInterviewHandler : IRequestHandler<EditInterviewCommand, Result>
    {
        private readonly IInterviewRespository _interviewRespository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditInterviewHandler(IInterviewRespository interviewRespository, IHttpContextAccessor contextAccessor)
        {
            _interviewRespository = interviewRespository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(EditInterviewCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the interviw
            var interview = await _interviewRespository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                return Result.Failure("interview does not exist", 404);
            }

            // step 2: edite the entity
            var participants = request.Participants.Select(
                    selector: x => InterviewParticipant.Create(
                            id: x.Id ?? Guid.NewGuid(),
                            interviewId: interview.Id,
                            userId: x.UserId,
                            role: x.Role
                        )
                ).ToList();

            interview.Update(
                    roundNumber: request.RoundNumber,
                    interviewType: request.InterviewType,
                    scheduledAt: request.ScheduledAt,
                    durationInMinutes: request.DurationInMinutes,
                    meetingLink: request.MeetingLink,
                    status: request.Status,
                    participants: participants
                );

            // step 3: persist the entity
            await _interviewRespository.UpdateAsync(interview, cancellationToken);

            // step 4: return the result
            return Result.Success();
        }
    }
}