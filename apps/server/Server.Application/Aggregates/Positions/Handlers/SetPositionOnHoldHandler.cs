using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class SetPositionOnHoldHandler : IRequestHandler<SetPositionOnHoldCommand, Result>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public SetPositionOnHoldHandler(IPositionRepository positionRepository, IHttpContextAccessor contextAccessor)
        {
            _positionRepository = positionRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(SetPositionOnHoldCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch existing position
            var position = await _positionRepository.GetByIdAsync(command.PositionId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundExeption("Position Not Found.");
            }

            // step 2: make move
            position.PutOnHold(Guid.Parse(userIdString), command.Comments);

            // step 3: persist move
            await _positionRepository.UpdateAsync(position, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}