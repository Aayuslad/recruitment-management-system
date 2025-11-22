using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Skills.Commands;
using Server.Application.Skills.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/skill")]
    public class SkillController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SkillController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills(CancellationToken cancellationToken)
        {
            var query = new GetSkillsQuery(null, null, null);
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditSkill(Guid id, [FromBody] EditSkillCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSkill(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteSkillCommand(id);
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}