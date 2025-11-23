using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Exeptions;
using Server.Application.Positions.Commands;
using Server.Core.Results;

namespace Server.Application.Positions.Handlers
{
    internal class DeletePositionBatchHandler : IRequestHandler<DeletePositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public DeletePositionBatchHandler(IPositionBatchRepository positionBatchRepository, IHttpContextAccessor contextAccessor)
        {
            _positionBatchRepository = positionBatchRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(DeletePositionBatchCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch existing position
            var position = await _positionBatchRepository.GetByIdAsync(command.BatchId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundExeption("Position Not Found.");
            }

            // step 2: soft delete
            position.Delete(Guid.Parse(userIdString));
            await _positionBatchRepository.UpdateAsync(position, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}