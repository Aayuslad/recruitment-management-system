using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Queries;
using Server.Application.Positions.Queries.DTOs;
using Server.Application.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Positions.Handlers
{
    public class GetPositionsHandler : IRequestHandler<GetPositionsQuery, Result<PositionsDetailDTO>>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionsHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<PositionsDetailDTO>> Handle(GetPositionsQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch positions
            var positions = await _positionRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var positionsDetailDTO = new PositionsDetailDTO();
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
                positionsDetailDTO.Positions.Add(positionSummaryDto);
            }

            // step 3: return result
            return Result<PositionsDetailDTO>.Success(positionsDetailDTO);
        }
    }
}