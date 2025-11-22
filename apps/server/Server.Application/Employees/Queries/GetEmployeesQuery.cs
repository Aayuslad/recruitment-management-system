using MediatR;

using Server.Application.Employees.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Employees.Queries
{
    public class GetEmployeesQuery : IRequest<Result<List<EmployeeDetailDTO>>>
    {
    }
}