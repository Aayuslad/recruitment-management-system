
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Events.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Events;

namespace Server.Application.Aggregates.Events.Handlers
{
    internal class CreateEventHandler : IRequestHandler<CreateEventCommand, Result>
    {
        private readonly IEventRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreateEventHandler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor)
        {
            _repository = eventRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: create entty
            var newEventId = Guid.NewGuid();

            // create job openings entity list
            var jobOpenings = request.JobOpenings.Select(
                    selector: x => EventJobOpening.Create(
                            eventId: newEventId,
                            jobOpeningId: x.JobOpeningId
                        )
                ).ToList();

            // create root entity
            var event_ = Event.Create(
                    id: newEventId,
                    createdBy: Guid.Parse(userIdString),
                    name: request.Name,
                    type: request.Type,
                    eventJobOpenings: jobOpenings
                );

            // step 2: persist entity
            await _repository.AddAsync(event_, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}