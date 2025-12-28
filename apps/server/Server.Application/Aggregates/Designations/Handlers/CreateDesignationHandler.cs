using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Designations;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class CreateDesignationHandler : IRequestHandler<CreateDesignationCommand, Result>
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
                throw new UnAuthorisedExeption();
            }

            // step 1: check if designation with this name exists
            var result = await _designationRepository.ExistsByNameAsync(command.Name, cancellationToken);
            if (result)
            {
                throw new ConflictExeption("Designation with this name already exists");
            }

            // step 2: create designation
            var newDesignationId = Guid.NewGuid();

            // create designation skills
            var dSkills = command.DesignationSkills.Select(
                    selector: x => DesignationSkill.Create(
                            designationId: newDesignationId,
                            skillId: x.SkillId,
                            skillType: x.SkillType
                        )
                ).ToList() ?? [];

            // create root entity designation
            var designation = Designation.Create(
                    id: newDesignationId,
                    createdBy: Guid.Parse(userIdString),
                    name: command.Name,
                    skills: dSkills
                );

            // step 3: presist
            await _designationRepository.AddAsync(designation, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}