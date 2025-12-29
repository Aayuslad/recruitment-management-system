using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Aggregates.Skills.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Aggregates.Skills.Handlers
{
    internal class DeleteSkillHandler : IRequestHandler<DeleteSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteSkillHandler(ISkillRepository skillRepository, IHttpContextAccessor httpContextAccessor)
        {
            _skillRepository = skillRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(DeleteSkillCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch existing skill
            var skill = await _skillRepository.GetByIdAsync(command.Id, cancellationToken);
            if (skill == null)
            {
                throw new NotFoundException("Skill Not Found.");
            }

            // step 2: delete skill
            skill.Delete(Guid.Parse(userIdString));
            await _skillRepository.UpdateAsync(skill, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}