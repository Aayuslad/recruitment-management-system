using MediatR;

using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Skills.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Aggregates.Skills.Handlers
{
    internal class DeleteSkillHandler : IRequestHandler<DeleteSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserContext _userContext;

        public DeleteSkillHandler(ISkillRepository skillRepository, IUserContext userContext)
        {
            _skillRepository = skillRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch existing skill
            var skill = await _skillRepository.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null)
            {
                throw new NotFoundException("Skill Not Found.");
            }

            // step 2: soft delete
            skill.Delete(_userContext.UserId);
            await _skillRepository.UpdateAsync(skill, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}