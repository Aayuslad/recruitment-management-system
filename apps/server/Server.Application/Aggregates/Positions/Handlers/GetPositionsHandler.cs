using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Queries;
using Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class GetPositionsHandler : IRequestHandler<GetPositionsQuery, Result<List<PositionSummaryDTO>>>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionsHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<List<PositionSummaryDTO>>> Handle(GetPositionsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch positions
            var positions = await _positionRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var positionsDto = new List<PositionSummaryDTO>();
            foreach (var position in positions)
            {
                var positionSummaryDto = new PositionSummaryDTO
                {
                    PositionId = position.Id,
                    BatchId = position.BatchId,
                    Descripcion = position.PositionBatch.Description,
                    DesignationId = position.PositionBatch.DesignationId,
                    DesignationName = position.PositionBatch.Designation.Name,
                    JobLocation = position.PositionBatch.JobLocation,
                    MinCTC = position.PositionBatch.MinCTC,
                    MaxCTC = position.PositionBatch.MaxCTC,
                    Status = position.Status,
                    ClosedByCandidate = position.ClosedByCandidate,
                    ClosureReason = position.ClosureReason,
                };
                positionsDto.Add(positionSummaryDto);
            }

            // step 3: return result
            return Result<List<PositionSummaryDTO>>.Success(positionsDto);
        }
    }
}