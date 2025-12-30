using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Queries;
using Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class GetBatchPositionsHandler : IRequestHandler<GetBatchPositionsQuery, Result<List<BatchPositionSummaryDTO>>>
    {
        private readonly IPositionRepository _positionRepository;

        public GetBatchPositionsHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<List<BatchPositionSummaryDTO>>> Handle(GetBatchPositionsQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch positions
            var positions = await _positionRepository.GetAllByBatchIdAsync(query.BatchId, cancellationToken);

            // step 2: map dto
            var positionsDto = new List<BatchPositionSummaryDTO>();
            foreach (var position in positions)
            {
                var positionSummaryDto = new BatchPositionSummaryDTO
                {
                    PositionId = position.Id,
                    BatchId = position.BatchId,
                    Status = position.Status,
                    ClosedByCandidateId = position.ClosedByCandidate,
                    ClosedByCandidateFullName = position.ClosedByCandidate != null ? $"{position.Candidate?.FirstName} {position.Candidate?.MiddleName} {position.Candidate?.LastName}" : null,
                    ClosureReason = position.ClosureReason,
                };
                positionsDto.Add(positionSummaryDto);
            }

            // step 3: return result
            return Result<List<BatchPositionSummaryDTO>>.Success(positionsDto);
        }
    }
}