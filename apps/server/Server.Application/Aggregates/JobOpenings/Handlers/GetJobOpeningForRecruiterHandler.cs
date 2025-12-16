
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobOpenings.Queries;
using Server.Application.Aggregates.JobOpenings.Queries.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Handlers
{
    internal class GetJobOpeningForRecruiterHandler : IRequestHandler<GetJobOpeningForRecruiterQuery, Result<JobOpeningDetailDTO>>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public GetJobOpeningForRecruiterHandler(IJobOpeningRepository jobOpeningRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
        }

        public async Task<Result<JobOpeningDetailDTO>> Handle(GetJobOpeningForRecruiterQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the job opening
            var jo = await _jobOpeningRepository.GetByIdAsync(request.JobOpeningId, cancellationToken);
            if (jo == null)
            {
                throw new NotFoundExeption("Job Opening Not Found.");
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

            // skill overrides for jo, (required for eidt jo)
            var joSkillOverRides = jo.SkillOverRides.Select(
                    selector: x => new SkillOverRideDetailDTO
                    {
                        Id = x.Id,
                        SkillId = x.SkillId,
                        Comments = x.Comments,
                        MinExperienceYears = x.MinExperienceYears,
                        Type = x.Type,
                        ActionType = x.ActionType,
                    }
                ).ToList();

            // interviewers
            var interviewers = jo.JobOpeningInterviewers.Select(
                    selector: x => new JobOpeningInterviewerDetailDTO
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        UserName = x.InterviewerUser.Auth.UserName,
                        Email = x.InterviewerUser.Auth.Email.ToString(),
                        Role = x.Role,
                    }
                ).ToList();

            // interview rounds
            var interviewRounds = jo.InterviewRounds.Select(
                    selector: x => new InterviewRoundTemplateDetailDTO
                    {
                        Id = x.Id,
                        Description = x.Description,
                        RoundNumber = x.RoundNumber,
                        DurationInMinutes = x.DurationInMinutes,
                        Type = x.Type,
                        Requirements = x.PanelRequirements.Select(
                                selector: x => new InterviewPanelRequirementDetailDTO
                                {
                                    Id = x.Id,
                                    Role = x.Role,
                                    RequirementCount = x.RequiredCount,
                                }
                            ).ToList()
                    }
                ).ToList();

            // aggregate root
            var jobOpeningDto = new JobOpeningDetailDTO
            {
                Id = jo.Id,
                Title = jo.Title,
                Description = jo.Description,
                Type = jo.Type,
                PositionBatchId = jo.PositionBatchId,
                DesignationId = jo.PositionBatch.DesignationId,
                DesignationName = jo.PositionBatch.Designation.Name,
                JobLocation = jo.PositionBatch.JobLocation,
                MinCTC = jo.PositionBatch.MinCTC,
                MaxCTC = jo.PositionBatch.MaxCTC,
                PositionsCount = jo.PositionBatch.Positions.Count,
                ClosedPositionsCount = jo.PositionBatch.Positions.Count(x => x.Status == PositionStatus.Closed),
                Skills = skills,
                SkillOverRides = joSkillOverRides,
                Interviewers = interviewers,
                InterviewRounds = interviewRounds,
            };

            // step 3: return result
            return Result<JobOpeningDetailDTO>.Success(jobOpeningDto);
        }
    }
}