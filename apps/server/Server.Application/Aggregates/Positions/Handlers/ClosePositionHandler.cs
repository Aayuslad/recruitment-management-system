using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class ClosePositionHandler : IRequestHandler<ClosePositionCommand, Result>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public ClosePositionHandler(IPositionRepository positionRepository, IHttpContextAccessor contextAccessor)
        {
            _positionRepository = positionRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(ClosePositionCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch existing position
            var position = await _positionRepository.GetByIdAsync(command.PositionId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundException("Position Not Found.");
            }

            // step 2: make move
            position.CloseWithoutCandidate(command.ClosureReason, Guid.Parse(userIdString));

            // step 3: persist move
            await _positionRepository.UpdateAsync(position, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}