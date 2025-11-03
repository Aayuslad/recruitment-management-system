using MediatR;

using Server.Core.Results;

namespace Server.Application.Positions.Commands
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