
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobOpenings.Queries;
using Server.Application.JobOpenings.Queries.DTOs;
using Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Handlers
{
    internal class GetJobOpeningForCandidateHandler : IRequestHandler<GetJobOpeningForCandiateQuery, Result<JobOpeningDetailDTO>>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public GetJobOpeningForCandidateHandler(IJobOpeningRepository jobOpeningRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
        }

        public async Task<Result<JobOpeningDetailDTO>> Handle(GetJobOpeningForCandiateQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch job opening
            var jo = await _jobOpeningRepository.GetByIdAsync(request.Id, cancellationToken);
            if (jo == null)
            {
                return Result<JobOpeningDetailDTO>.Failure("Job Opening not found", 404);
            }

            // step 2: map dto

            // skills of designation (the source)
            var skills = jo.PositionBatch.Designation.DesignationSkills.Select(
                selector: ds => new SkillDetailDTO
                {
                    SkillId = ds.SkillId,
                    SkillName = ds.Skill.Name,
                    SkillType = ds.SkillType,
                    MinExperienceYears = ds.MinExperienceYears,
                }
                ).ToList();

            // skill over rides for position
            foreach (var overRide in jo.PositionBatch.SkillOverRides)
            {
                switch (overRide.ActionType)
                {
                    case SkillActionType.Add:
                        skills.Add(new SkillDetailDTO
                        {
                            SkillId = overRide.SkillId,
                            SkillName = overRide.Skill.Name,
                            MinExperienceYears = overRide.MinExperienceYears,
                            SkillType = overRide.Type,
                        });
                        break;

                    case SkillActionType.Update:
                        var skill = skills.FirstOrDefault(x => x.SkillId == overRide.SkillId);
                        if (skill != null)
                        {
                            skill.MinExperienceYears = overRide.MinExperienceYears;
                            skill.SkillType = overRide.Type;
                        }
                        break;

                    case SkillActionType.Remove:
                        skills.RemoveAll(x => x.SkillId == overRide.SkillId);
                        break;
                }
            }

            // skill over rides for job opening
            foreach (var overRide in jo.SkillOverRides)
            {
                switch (overRide.ActionType)
                {
                    case SkillActionType.Add:
                        skills.Add(new SkillDetailDTO
                        {
                            SkillId = overRide.SkillId,
                            SkillName = overRide.Skill.Name,
                            MinExperienceYears = overRide.MinExperienceYears,
                            SkillType = overRide.Type,
                        });
                        break;

                    case SkillActionType.Update:
                        var skill = skills.FirstOrDefault(x => x.SkillId == overRide.SkillId);
                        if (skill != null)
                        {
                            skill.MinExperienceYears = overRide.MinExperienceYears;
                            skill.SkillType = overRide.Type;
                        }
                        break;

                    case SkillActionType.Remove:
                        skills.RemoveAll(x => x.SkillId == overRide.SkillId);
                        break;
                }
            }

            var joDto = new JobOpeningDetailDTO
            {
                Id = jo.Id,
                Title = jo.Title,
                Description = jo.Description,
                DesignationName = jo.PositionBatch.Designation.Name,
                Type = jo.Type,
                Skills = skills
            };

            // return result
            return Result<JobOpeningDetailDTO>.Success(joDto);
        }
    }
}