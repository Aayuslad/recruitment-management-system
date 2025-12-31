using MediatR;

using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Skills.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Aggregates.Skills.Handlers
{
    internal class EditSkillHandler : IRequestHandler<EditSkillCommand, Result>
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IUserContext _userContext;

        public EditSkillHandler(ISkillRepository skillRepository, IUserContext userContext)
        {
            _skillRepository = skillRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditSkillCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch existing skill
            var skill = await _skillRepository.GetByIdAsync(request.Id, cancellationToken);
            if (skill == null)
            {
                throw new NotFoundException("Role Not Found.");
            }

            // step 2: update skill properties
            skill.Update(
                request.Name,
                _userContext.UserId
            );

            // step 3: persist changes
            await _skillRepository.UpdateAsync(skill, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}