using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Skills.Commands;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    public class EditSkillHandler : IRequestHandler<EditSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditSkillHandler(ISkillRepository skillRepository, IHttpContextAccessor httpContextAccessor)
        {
            _skillRepository = skillRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(EditSkillCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch existing skill
            var skill = await _skillRepository.GetByIdAsync(command.Id, cancellationToken);
            if (skill == null)
            {
                return Result.Failure("Skill not found", 404);
            }

            // step 2: update skill properties
            skill.UpdateDetails(
                command.Name,
                command.Description,
                Guid.Parse(userIdString)
            );

            // step 3: persist changes
            await _skillRepository.UpdateAsync(skill, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}