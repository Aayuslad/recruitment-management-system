using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class DeleteDesignationHandler : IRequestHandler<DeleteDesignationCommand, Result>
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
                throw new UnAuthorisedException();
            }

            // step 1: check if designation exists
            var designation = await _designationRepository.GetByIdAsync(command.Id, cancellationToken);
            if (designation == null)
            {
                throw new NotFoundException($"Designation not found.");
            }

            //TODO: if designation is used in any of Position or other parent entity, throw conflict Exception

            // step 2: delete designation
            designation.Delete(Guid.Parse(userIdString));

            // step 3: persist chages
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 4: return success
            return Result.Success();
        }
    }
}