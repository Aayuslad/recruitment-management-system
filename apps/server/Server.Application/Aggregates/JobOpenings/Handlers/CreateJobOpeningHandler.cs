
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.JobOpenings.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Entities.JobOpenings;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Handlers
{
    internal class CreateJobOpeningHandler : IRequestHandler<CreateJobOpeningCommand, Result>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IUserContext _userContext;

        public CreateJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IUserContext userContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreateJobOpeningCommand request, CancellationToken cancellationToken)
        {
            // step 1: create entity
            var newJobOpeningId = Guid.NewGuid();

            // create skil over rides entity list
            var skillOverRides = request.SkillOverRides.Select(
                    selector: x => SkillOverRide.CreateForJobOpening(
                            id: null,
                            jobOpeningId: newJobOpeningId,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.JobOpening
                        )
                ).ToList();

            // create interviewer enity list
            var interviewers = request.Interviewers.Select(
                    selector: x => JobOpeningInterviewer.Create(
                            id: null,
                            jobOpeningId: newJobOpeningId,
                            userId: x.UserId,
                            role: x.Role
                        )
                ).ToList();

            // create interview rounds
            var interviewRoundsId = Guid.NewGuid();
            var interviewRounds = request.InterviewRounds.Select(
                selector: x => InterviewRoundTemplate.Create(
                        id: interviewRoundsId,
                        jobOpeningId: newJobOpeningId,
                        description: x.Description,
                        roundNumber: x.RoundNumber,
                        type: x.Type,
                        durationInMinutes: x.DurationInMinutes,
                        panelRequirements: x.Requirements.Select(
                                selector: y => InterviewPanelRequirement.Create(
                                        id: null,
                                        interviewTemplateId: interviewRoundsId,
                                        role: y.Role,
                                        requiredCount: y.RequirementCount
                                    )
                            ).ToList()
                    )
                ).ToList();

            // create aggregate root entity
            var jobOpening = JobOpening.Create(
                    id: newJobOpeningId,
                    createdBy: _userContext.UserId,
                    positionBatchId: request.PositionBatchId,
                    title: request.Title,
                    description: request.Description,
                    type: request.Type,
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