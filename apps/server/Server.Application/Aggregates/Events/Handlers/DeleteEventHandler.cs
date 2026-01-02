
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Events.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Events.Handlers
{
    internal class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Result>
    {
        private readonly IEventRepository _repository;
        private readonly IUserContext _userContext;

        public DeleteEventHandler(IEventRepository eventRepository, IUserContext userContext)
        {
            _repository = eventRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch event
            var event_ = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (event_ is null)
            {
                throw new NotFoundException("Event Not Found");
            }

            // step 2: soft delete
            event_.Delete(_userContext.UserId);

            // step 3: perist changes
            await _repository.UpdateAsync(event_, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}