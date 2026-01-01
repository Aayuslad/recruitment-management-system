using MediatR;

using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Skills.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Skills;
using Server.Infrastructure.Repositories;

namespace Server.Application.Aggregates.Skills.Handlers
{
    internal class CreateSkillHandler : IRequestHandler<CreateSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserContext _userContext;

        public CreateSkillHandler(ISkillRepository skillRepository, IUserContext userContext)
        {
            _skillRepository = skillRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateSkillCommand request, CancellationToken cancellationToken)
        {
            // step 1: check alredy existing skill with name
            var nameResult = await _skillRepository.ExistsByNameAsync(request.Name, cancellationToken);
            if (nameResult)
            {
                throw new ConflictException($"Skill with name {request.Name} already exists.");
            }

            // step 2: create and persist entiry
            var skill = Skill.Create(
                request.Name,
                _userContext.UserId
            );
            await _skillRepository.AddAsync(skill, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}