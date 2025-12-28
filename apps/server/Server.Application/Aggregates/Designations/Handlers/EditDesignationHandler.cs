using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Designations;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class EditDesignationHandler : IRequestHandler<EditDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditDesignationHandler(IDesignationRepository designationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _designationRepository = designationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(EditDesignationCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch existing 
            var designation = await _designationRepository.GetByIdAsync(command.Id, cancellationToken);
            if (designation == null)
            {
                throw new NotFoundExeption($"Designation not found.");
            }

            // step 2: edit skill

            // create new list of eidted
            var newDSkills = command.DesignationSkills?.Select(
                    selector: x => DesignationSkill.Create(
                            designationId: designation.Id,
                            skillId: x.SkillId,
                            skillType: x.SkillType
                        )
                ).ToList() ?? [];

            // update root entity
            designation.Update(
                updatedBy: Guid.Parse(userIdString),
                name: command.Name,
                newSkills: newDSkills
            );

            // step 3: persist changes
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 4: return success
            return Result.Success();
        }
    }
}