using MediatR;

using Server.Application.Aggregates.Skills.Queries;
using Server.Application.Aggregates.Skills.Queries.DTOs;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Aggregates.Skills.Handlers
{
    internal class GetSkillsHandler : IRequestHandler<GetSkillsQuery, Result<IEnumerable<SkillDetailDTO>>>
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillsHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Result<IEnumerable<SkillDetailDTO>>> Handle(GetSkillsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch skills
            var skills = await _skillRepository.GetAllAsync(cancellationToken);

            // step 2: map to DTOs
            var skillDtos = skills.Select(s => new SkillDetailDTO
            {
                Id = s.Id,
                Name = s.Name,
            });

            // step 3: return success
            return Result<IEnumerable<SkillDetailDTO>>.Success(skillDtos);
        }
    }
}