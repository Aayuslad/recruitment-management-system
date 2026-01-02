using MediatR;

using Server.Core.Results;

namespace Server.Application.Aggregates.Positions.Commands
{
    public class DeletePositionBatchCommand : IRequest<Result>
    {
        public DeletePositionBatchCommand(Guid batchId)
        {
            BatchId = batchId;
        }

        public Guid BatchId { get; set; }
    }
}