using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class DeleteDesignationHandler : IRequestHandler<DeleteDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IUserContext _userContext;

        public DeleteDesignationHandler(IUserContext userContext, IDesignationRepository designationRepository)
        {
            _userContext = userContext;
            _designationRepository = designationRepository;
        }

        public async Task<Result> Handle(DeleteDesignationCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if designation exists
            var designation = await _designationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (designation == null)
            {
                throw new NotFoundException($"Designation not found.");
            }

            //TODO: if designation is used in any of Position or other parent entity, throw conflict Exception

            // step 2: delete designation
            designation.Delete(_userContext.UserId);

            // step 3: persist chages
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 4: return success
            return Result.Success();
        }
    }
}