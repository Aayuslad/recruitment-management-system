using MediatR;

using Server.Application.DTOs;
using Server.Application.Skills.Queries;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    public class GetSkillHandler : IRequestHandler<GetSkillQuery, Result<SkillDTO>>
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Result<SkillDTO>> Handle(GetSkillQuery query, CancellationToken cancellationToken)
        {
            var skillId = query.Id;

            // step 1: fetch the skill from db
            var skill = await _skillRepository.GetByIdAsync(skillId, cancellationToken);
            if (skill == null)
            {
                return Result<SkillDTO>.Failure("Skill not found", 404);
            }

            // step 2: create dto
            var skillDto = new SkillDTO
            {
                Id = skill.Id,
                Name = skill.Name,
                Description = skill.Description,
                CreatedAt = skill.CreatedAt,
            };

            // step 3: return dto
            return Result<SkillDTO>.Success(skillDto);
        }
    }
}
