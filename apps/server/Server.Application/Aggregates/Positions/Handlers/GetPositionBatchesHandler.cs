using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Queries;
using Server.Application.Aggregates.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class GetPositionBatchesHandler : IRequestHandler<GetPositionBatchesQuery, Result<List<PositionBatchSummaryDTO>>>
    {
        private readonly IPositionBatchRepository _batchRepository;

        public GetPositionBatchesHandler(IPositionBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result<List<PositionBatchSummaryDTO>>> Handle(GetPositionBatchesQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch positinoBatch
            var batches = await _batchRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var batchesDto = new List<PositionBatchSummaryDTO>();
            foreach (var batch in batches)
            {
                var summaryDto = new PositionBatchSummaryDTO
                {
                    BatchId = batch.Id,
                    Description = batch.Description,
                    DesignationId = batch.DesignationId,
                    DesignationName = batch.Designation.Name,
                    JobLocation = batch.JobLocation,
                    MinCTC = batch.MinCTC,
                    MaxCTC = batch.MaxCTC,
                    PositionsCount = batch.Positions.Count,
                    ClosedPositionsCount = batch.Positions.Count(x => x.Status == PositionStatus.Closed),
                    PositionsOnHoldCount = batch.Positions.Count(x => x.Status == PositionStatus.OnHold),
                    CreatedBy = batch.CreatedBy,
                    CreatedByUserName = batch.CreatedByUser?.Auth.UserName,
                };
                batchesDto.Add(summaryDto);
            }

            // step 3: return result
            return Result<List<PositionBatchSummaryDTO>>.Success(batchesDto);
        }
    }
}