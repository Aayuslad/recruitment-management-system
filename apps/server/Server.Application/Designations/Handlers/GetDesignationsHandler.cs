using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Designations.Queries;
using Server.Application.Designations.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Designations.Handlers
{
    internal class GetDesignationsHandler : IRequestHandler<GetDesignationsQuery, Result<IEnumerable<DesignationDetailDTO>>>
    {
        private readonly IDesignationRepository _designationRepository;

        public GetDesignationsHandler(IDesignationRepository designationRepository)
        {
            _designationRepository = designationRepository;
        }

        public async Task<Result<IEnumerable<DesignationDetailDTO>>> Handle(GetDesignationsQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch designations
            var designations = await _designationRepository.GetAllAsync(cancellationToken);

            // step 2: map to DTOs
            var designationDtos = designations.Select(designation => new DesignationDetailDTO
            {
                Id = designation.Id,
                Name = designation.Name,
                DesignationSkills = designation.DesignationSkills?.Select(ds => new DesignationSkillDetailDTO
                {
                    SkillId = ds.SkillId,
                    SkillType = ds.SkillType,
                    Name = ds.Skill.Name,
                    MinExperienceYears = ds.MinExperienceYears,
                }).ToList(),
                CreatedBy = designation.CreatedBy,
                CreatedAt = designation.CreatedAt,
                LastUpdatedBy = designation.LastUpdatedBy,
                LastUpdatedAt = designation.LastUpdatedAt
            });

            // step 3: return success
            return Result<IEnumerable<DesignationDetailDTO>>.Success(designationDtos);
        }
    }
}