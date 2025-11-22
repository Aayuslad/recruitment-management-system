using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Employees.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDesignations(CancellationToken cancellationToken)
        {
           var query = new GetEmployeesQuery();
           var result = await _mediator.Send(query, cancellationToken);
           return result.ToActionResult(this);
        }
    }
}