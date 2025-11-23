
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Events.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Events.Handlers
{
    internal class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Result>
    {
        private readonly IEventRepository _repository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeleteEventHandler(IEventRepository eventRepository, IHttpContextAccessor contextAccessor)
        {
            _repository = eventRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch event
            var event_ = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (event_ is null)
            {
                throw new NotFoundExeption("Event Not Found");
            }

            // step 2: soft delete
            event_.Delete(Guid.Parse(userIdString));

            // step 3: perist changes
            await _repository.UpdateAsync(event_, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}