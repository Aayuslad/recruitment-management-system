using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Designations.Commands;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Designations.Handlers
{
    public class EditDesignationHandler : IRequestHandler<EditDesignationCommand, Result>
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
            var designationSkills = command.DesignationSkills?
                .Select(ds => DesignationSkill.Create(
                    designation.Id,
                    ds.SkillId,
                    ds.SkillType,
                    ds.MinExperienceYears
                ))
                .ToList() ?? new List<DesignationSkill>();

            designation.Update(
                command.Name,
                command.Description,
                designationSkills,
                Guid.Parse(userIdString)
            );

            // step 3: persist changes
            await _designationRepository.UpdateAsync(designation, cancellationToken);

            // step 4: return success
            return Result.Success();
        }
    }
}