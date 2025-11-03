using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Designations.Commands;
using Server.Application.Designations.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Entities;

namespace Server.Application.Designations.Handlers
{
    public class CreateDesignationHandler : IRequestHandler<CreateDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateDesignationHandler(IDesignationRepository designationRepository, IHttpContextAccessor contextAccessor)
        {
            _designationRepository = designationRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateDesignationCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check if designation with this name exists
            var result = await _designationRepository.ExistsByNameAsync(command.Name, cancellationToken);
            if (result)
            {
                return Result.Failure("Designation with this name already exists", 409);
            }

            // step 2: create designation
            var designation = Designation.Create(
                    command.Name,
                    command.Description,
                    Guid.Parse(userIdString)
                );

            if (command.DesignationSkills?.Count > 0)
            {
                foreach (var skill in command.DesignationSkills)
                {
                    designation.AddSkill(
                        DesignationSkill.Create(
                            designation.Id,
                            skill.SkillId,
                            skill.SkillType,
                            skill.MinExperienceYears
                        )
                    );
                }
            }

            // step 3: presist
            await _designationRepository.AddAsync(designation, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}