using MediatR;

using Server.Application.Skills.Queries;
using Server.Application.Skills.Queries.DTOs;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    public class GetSkillsHandler : IRequestHandler<GetSkillsQuery, Result<IEnumerable<SkillDetailDTO>>>
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillsHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Result<IEnumerable<SkillDetailDTO>>> Handle(GetSkillsQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch skills
            var skills = await _skillRepository.GetAllAsync(cancellationToken);

            // step 2: map to DTOs
            var skillDtos = skills.Select(s => new SkillDetailDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedBy = s.CreatedBy,
                CreatedAt = s.CreatedAt,
                LastUpdatedAt = s.LastUpdatedAt,
                LastUpdatedBy = s.LastUpdatedBy,
            });

            // step 3: return success
            return Result<IEnumerable<SkillDetailDTO>>.Success(skillDtos);
        }
    }
}