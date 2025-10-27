using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Skills.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    public class CreateSkillHandler: IRequestHandler<CreateSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateSkillHandler(ISkillRepository skillRepository, IHttpContextAccessor httpContextAccessor)
        {
            _skillRepository = skillRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(CreateSkillCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check alredy existing skill with name
            var nameResult = await _skillRepository.ExistsByNameAsync(command.Name, cancellationToken);
            if (nameResult)
            {
                return Result.Failure("Skill with this name alredy exists", 409);
            }

            // step 2: create and persist entiry
            var skill = Skill.Create(
                command.Name,
                command.Description,
                Guid.Parse(userIdString)
            );
            await _skillRepository.AddAsync(skill, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}
