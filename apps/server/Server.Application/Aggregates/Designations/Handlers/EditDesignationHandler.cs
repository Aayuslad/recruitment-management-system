using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Designations;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class EditDesignationHandler : IRequestHandler<EditDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IUserContext _userContext;

        public EditDesignationHandler(IDesignationRepository designationRepository, IUserContext userContext)
        {
            _designationRepository = designationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditDesignationCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch existing 
            var designation = await _designationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (designation == null)
            {
                throw new NotFoundException($"Designation not found.");
            }

            // step 2: edit skill

            // create new list of eidted
            var newDSkills = request.DesignationSkills?.Select(
                    selector: x => DesignationSkill.Create(
                            designationId: designation.Id,
                            skillId: x.SkillId,
                            skillType: x.SkillType
                        )
                ).ToList() ?? [];

            // update root entity
            designation.Update(
                updatedBy: _userContext.UserId,
                name: request.Name,
                newSkills: newDSkills
            );

            // step 3: persist changes
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 4: return success
            return Result.Success();
        }
    }
}