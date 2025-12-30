using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Aggregates.Positions.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/positions")]
    public class PositionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PositionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // position batches
        [HttpPost("batches")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> CreatePositionBatch([FromBody] CreatePositionBatchCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("batches/{batchId:Guid}")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> EditPositionBatch(Guid batchId, EditPositionBatchCommand command, CancellationToken cancellationToken)
        {
            command.PositionBatchId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("batches/{batchId:Guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePositionbatch(Guid batchId, CancellationToken cancellationToken)
        {
            var command = new DeletePositionBatchCommand(batchId);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("batches/{batchId:Guid}")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetPositionBatch(Guid batchId, CancellationToken cancellationToken)
        {
            var query = new GetPositionBatchQuery(batchId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("batches")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetPositionBatches(CancellationToken cancellationToken)
        {
            var query = new GetPositionBatchesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        // positions

        [HttpPatch("{positionId:Guid}/close")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> ClosePosition(Guid positionId, [FromBody] ClosePositionCommand command, CancellationToken cancellationToken)
        {
            command.PositionId = positionId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPatch("{positionId:Guid}/set-on-hold")]
        [Authorize(Roles = "Admin, Recruiter")]
        public async Task<IActionResult> SetPositionOnHold(Guid positionId, SetPositionOnHoldCommand command, CancellationToken cancellationToken)
        {
            command.PositionId = positionId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{positionId:Guid}")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetPosition(Guid positionId, CancellationToken cancellationToken)
        {
            var query = new GetPositionQuery(positionId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        // TODO: not used, remove at end of V1
        public async Task<IActionResult> GetPositions(CancellationToken cancellationToken)
        {
            var query = new GetPositionsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("batches/{batchId:Guid}/positions")]
        [Authorize(Roles = "Admin, Recruiter, Viewer")]
        public async Task<IActionResult> GetBatchPositions(Guid batchId, CancellationToken cancellationToken)
        {
            var query = new GetBatchPositionsQuery(batchId);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}