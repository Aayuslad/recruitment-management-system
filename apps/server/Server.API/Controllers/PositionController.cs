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
    [Route("api/position")]
    public class PositionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PositionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // position batches
        [HttpPost("batch")]
        public async Task<IActionResult> CreatePositionBatch([FromBody] CreatePositionBatchCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("batch/{id:Guid}")]
        public async Task<IActionResult> EditPositionBatch(Guid id, EditPositionBatchCommand command, CancellationToken cancellationToken)
        {
            command.PositionBatchId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("batch/{id:Guid}")]
        public async Task<IActionResult> DeletePositionbatch(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeletePositionBatchCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("batch/{id:Guid}")]
        public async Task<IActionResult> GetPositionBatch(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetPositionBatchQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("batch")]
        public async Task<IActionResult> GetPositionBatches(CancellationToken cancellationToken)
        {
            var query = new GetPositionBatchesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        // positions

        [HttpPut("/close/{id:Guid}")]
        public async Task<IActionResult> ClosePosition(Guid id, [FromBody] ClosePositionCommand command, CancellationToken cancellationToken)
        {
            command.PositionId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("/onHold/{id:Guid}")]
        public async Task<IActionResult> SetPositionOnHold(Guid id, SetPositionOnHoldCommand command, CancellationToken cancellationToken)
        {
            command.PositionId = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetPosition(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetPositionQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetPositions(CancellationToken cancellationToken)
        {
            var query = new GetPositionsQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}