using MediatR;

using Server.Application.Skills.Queries;
using Server.Application.Skills.Queries.DTOs;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    internal class GetSkillHandler : IRequestHandler<GetSkillQuery, Result<SkillDetailDTO>>
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Result<SkillDetailDTO>> Handle(GetSkillQuery query, CancellationToken cancellationToken)
        {
            var skillId = query.Id;

            // step 1: fetch the skill from db
            var skill = await _skillRepository.GetByIdAsync(skillId, cancellationToken);
            if (skill == null)
            {
                return Result<SkillDetailDTO>.Failure("Skill not found", 404);
            }

            // step 2: create dto
            var skillDto = new SkillDetailDTO
            {
                Id = skill.Id,
                Name = skill.Name,
            };

            // step 3: return dto
            return Result<SkillDetailDTO>.Success(skillDto);
        }
    }
}