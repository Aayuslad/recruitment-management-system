using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Employees.Queries;
using Server.Application.Employees.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Employees.Handlers
{
    internal class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, Result<List<EmployeeDetailDTO>>>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public GetEmployeesHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Result<List<EmployeeDetailDTO>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            // step 1: get all employees
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);

            // step 2: list dtos
            var employeeDtos = employees.Select(
                selector: x => new EmployeeDetailDTO
                {
                    Id = x.Id,
                    DesignationId = x.DesignationId,
                    DesignationName = x.Designation.Name,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    ContactNumber = x.ContactNumber,
                    Dob = x.Dob,
                }
            ).ToList();

            // step 3: return result
            return Result<List<EmployeeDetailDTO>>.Success(employeeDtos);
        }
    }
}
