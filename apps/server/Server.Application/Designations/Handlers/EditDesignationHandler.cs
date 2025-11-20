using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Designations.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Designations.Handlers
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
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch existing 
            var designation = await _designationRepository.GetByIdAsync(command.Id, cancellationToken);
            if (designation == null)
            {
                return Result.Failure("Designation not found", 404);
            }

            // step 2: edit skill

            // create new list of eidted
            var newDSkills = command.DesignationSkills?.Select(
                    selector: x => DesignationSkill.Create(
                            designationId: designation.Id,
                            skillId: x.SkillId,
                            skillType: x.SkillType,
                            minExperienceYears: x.MinExperienceYears
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