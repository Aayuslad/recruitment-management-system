
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Interviews.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Interviews;

namespace Server.Application.Aggregates.Interviews.Handlers
{
    internal class EditInterviewHandler : IRequestHandler<EditInterviewCommand, Result>
    {
        private readonly IInterviewRepository _interviewRepository;
        private readonly IUserContext _userContext;

        public EditInterviewHandler(IInterviewRepository interviewRepository, IUserContext userContext)
        {
            _interviewRepository = interviewRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditInterviewCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the interviw
            var interview = await _interviewRepository.GetByIdAsync(request.Id, cancellationToken);
            if (interview is null)
            {
                throw new NotFoundException("Interview Not Found.");
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
            await _interviewRepository.UpdateAsync(interview, cancellationToken);

            // step 4: return the result
            return Result.Success();
        }
    }
}