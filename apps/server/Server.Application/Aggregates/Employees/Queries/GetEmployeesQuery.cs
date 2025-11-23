using MediatR;

using Server.Application.Aggregates.Employees.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Employees.Queries
{
    public class GetEmployeesQuery : IRequest<Result<List<EmployeeDetailDTO>>>
    {
    }
}