using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Designations.Queries;
using Server.Application.Aggregates.Designations.Queries.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Handlers
{
    internal class GetDesignationHandler : IRequestHandler<GetDesignationQuery, Result<DesignationDetailDTO>>
    {
        private readonly IDesignationRepository _designationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDesignationHandler(IDesignationRepository designationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _designationRepository = designationRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<DesignationDetailDTO>> Handle(GetDesignationQuery query, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch designation
            var designation = await _designationRepository.GetByIdAsync(query.Id, cancellationToken);
            if (designation == null)
            {
                throw new NotFoundExeption("Designation not found");
            }

            // step 2: map to DTO
            var designationDto = new DesignationDetailDTO
            {
                Id = designation.Id,
                Name = designation.Name,
                DesignationSkills = designation.DesignationSkills?.Select(ds => new DesignationSkillDetailDTO
                {
                    SkillId = ds.SkillId,
                    SkillType = ds.SkillType,
                    Name = ds.Skill.Name,
                }).ToList(),
            };

            // step 3: return success
            return Result<DesignationDetailDTO>.Success(designationDto);
        }
    }
}