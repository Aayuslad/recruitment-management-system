using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class SetPositionOnHoldHandler : IRequestHandler<SetPositionOnHoldCommand, Result>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IUserContext _userContext;

        public SetPositionOnHoldHandler(IPositionRepository positionRepository, IUserContext userContext)
        {
            _positionRepository = positionRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(SetPositionOnHoldCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch existing position
            var position = await _positionRepository.GetByIdAsync(request.PositionId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundException("Position Not Found.");
            }

            // step 2: make move
            position.PutOnHold(_userContext.UserId, request.Comments);

            // step 3: persist move
            await _positionRepository.UpdateAsync(position, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}