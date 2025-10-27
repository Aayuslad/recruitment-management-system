using MediatR;

using Server.Application.DTOs;
using Server.Application.Skills.Queries;
using Server.Core.Results;
using Server.Infrastructure.Repositories;

namespace Server.Application.Skills.Handlers
{
    public class GetSkillsHandler : IRequestHandler<GetSkillsQuery, Result<IEnumerable<SkillDTO>>>
    {
        private readonly ISkillRepository _skillRepository;

        public GetSkillsHandler(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<Result<IEnumerable<SkillDTO>>> Handle(GetSkillsQuery query, CancellationToken cancellationToken)
        {
            var skills = await _skillRepository.GetAllAsync(cancellationToken);

            if (skills == null || !skills.Any())
                return Result<IEnumerable<SkillDTO>>.Failure("No skills found", 404);

            var skillDtos = skills.Select(s => new SkillDTO
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                CreatedAt = s.CreatedAt,
            });

            return Result<IEnumerable<SkillDTO>>.Success(skillDtos);
        }
    }
}
