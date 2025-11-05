using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Queries;
using Server.Application.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Positions.Handlers
{
    public class GetPositionBatchesHandler : IRequestHandler<GetPositionBatchesQuery, Result<PositionBatchesDetailDTO>>
    {
        private readonly IPositionBatchRepository _batchRepository;

        public GetPositionBatchesHandler(IPositionBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result<PositionBatchesDetailDTO>> Handle(GetPositionBatchesQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch positinoBatch
            var batches = await _batchRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var batchesDetailDTO = new PositionBatchesDetailDTO();
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
                batchesDetailDTO.Batches.Add(summaryDto);
            }

            // step 3: return result
            return Result<PositionBatchesDetailDTO>.Success(batchesDetailDTO);
        }
    }
}