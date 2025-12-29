
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Events.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Events;

namespace Server.Application.Aggregates.Events.Handlers
{
    internal class EditEventHandler : IRequestHandler<EditEventCommand, Result>
    {
        private readonly IEventRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditEventHandler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor)
        {
            _repository = eventRepository;
            _contextAccessor = contextAccessor;

        }
        public async Task<Result> Handle(EditEventCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch event
            var event_ = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (event_ is null)
            {
                throw new NotFoundException("Event Not Found");
            }

            // step 2: make changes
            event_.Update(
                    updatedBy: Guid.Parse(userIdString),
                    name: request.Name,
                    type: request.Type,
                    eventJobOpenings: request.JobOpenings.Select(
                            selector: x => EventJobOpening.Create(
                                    eventId: event_.Id,
                                    jobOpeningId: x.JobOpeningId
                                )
                        ).ToList()
                );

            // step 3: persist entity
            await _repository.UpdateAsync(event_, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}