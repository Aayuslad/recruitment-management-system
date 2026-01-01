using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class DeletePositionBatchHandler : IRequestHandler<DeletePositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IUserContext _userContext;

        public DeletePositionBatchHandler(IPositionBatchRepository positionBatchRepository, IUserContext userContext)
        {
            _positionBatchRepository = positionBatchRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeletePositionBatchCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch existing position
            var position = await _positionBatchRepository.GetByIdAsync(request.BatchId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundException("Position Not Found.");
            }

            // step 2: soft delete
            position.Delete(_userContext.UserId);
            await _positionBatchRepository.UpdateAsync(position, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}