using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Designations;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class CreateDesignationHandler : IRequestHandler<CreateDesignationCommand, Result>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IUserContext _userContext;

        public CreateDesignationHandler(IDesignationRepository designationRepository, IUserContext userContext)
        {
            _designationRepository = designationRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateDesignationCommand request, CancellationToken cancellationToken)
        {
            // step 1: check if designation with this name exists
            var result = await _designationRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (result)
            {
                throw new ConflictException("Designation with this name already exists");
            }

            // step 2: create designation
            var newDesignationId = Guid.NewGuid();

            // create designation skills
            var dSkills = request.DesignationSkills.Select(
                    selector: x => DesignationSkill.Create(
                            designationId: newDesignationId,
                            skillId: x.SkillId,
                            skillType: x.SkillType
                        )
                ).ToList() ?? [];

            // create root entity designation
            var designation = Designation.Create(
                    id: newDesignationId,
                    createdBy: _userContext.UserId,
                    name: request.Name,
                    skills: dSkills
                );

            // step 3: presist
            await _designationRepository.AddAsync(designation, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}