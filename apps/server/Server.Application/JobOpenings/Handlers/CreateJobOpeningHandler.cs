
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobOpenings.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Handlers
{
    internal class CreateJobOpeningHandler : IRequestHandler<CreateJobOpeningCommand, Result>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IHttpContextAccessor httpContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _httpContextAccessor = httpContext;
        }

        public async Task<Result> Handle(CreateJobOpeningCommand cmd, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: create entity
            var newJobOpeningId = Guid.NewGuid();

            // create skil over rides entity list
            var skillOverRides = cmd.SkillOverRides.Select(
                    selector: x => SkillOverRide.CreateForJobOpening(
                            id: null,
                            jobOpeningId: newJobOpeningId,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            minExperienceYears: x.MinExperienceYears,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.JobOpening
                        )
                ).ToList();

            // create interviewer enity list
            var interviewers = cmd.Interviewers.Select(
                    selector: x => JobOpeningInterviewer.Create(
                            id: null,
                            jobOpeningId: newJobOpeningId,
                            userId: x.UserId,
                            role: x.Role
                        )
                ).ToList();

            // create interview rounds
            var interviewRoundsId = Guid.NewGuid();
            var interviewRounds = cmd.InterviewRounds.Select(
                selector: x => JobOpeningInterviewRoundTemplate.Create(
                        id: interviewRoundsId,
                        jobOpeningId: newJobOpeningId,
                        description: x.Description,
                        roundNumber: x.RoundNumber,
                        type: x.Type,
                        durationInMinutes: x.DurationInMinutes,
                        panelRequirements: x.Requirements.Select(
                                selector: y => JobOpeningInterviewPanelRequirement.Create(
                                        id: null,
                                        jobOpeningInterviewTemplateId: interviewRoundsId,
                                        role: y.Role,
                                        requiredCount: y.RequirementCount
                                    )
                            ).ToList()
                    )
                ).ToList();

            // create aggregate root entity
            var jobOpening = JobOpening.Create(
                    id: newJobOpeningId,
                    createdBy: Guid.Parse(userIdString),
                    positionBatchId: cmd.PositionBatchId,
                    title: cmd.Title,
                    description: cmd.Description,
                    type: cmd.Type,
                    jobOpeningInterviewers: interviewers,
                    interviewRounds: interviewRounds,
                    skillOverRides: skillOverRides
                );

            // step 2: persist entity
            await _jobOpeningRepository.AddAsync(jobOpening, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}