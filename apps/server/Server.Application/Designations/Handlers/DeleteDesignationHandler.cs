using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Designations.Commands;
using Server.Core.Results;

namespace Server.Application.Designations.Handlers
{
    public class DeleteDesignationHandler : IRequestHandler<DeleteDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteDesignationHandler(IHttpContextAccessor httpContextAccessor, IDesignationRepository designationRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _designationRepository = designationRepository;
        }

        public async Task<Result> Handle(DeleteDesignationCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if designation with this name exists
            var designation = await _designationRepository.GetByIdAsync(command.Id, cancellationToken);
            if (designation == null)
            {
                return Result.Failure("Designation not found", 409);
            }

            // step 2: delete designation
            designation.Delete(Guid.Parse(userIdString));
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 3: return success
            return Result.Success();
        }
    }
}