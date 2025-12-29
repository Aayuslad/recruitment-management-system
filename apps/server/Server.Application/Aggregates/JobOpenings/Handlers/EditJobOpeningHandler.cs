
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobOpenings.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Entities.JobOpenings;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.JobOpenings.Handlers
{
    internal class EditJobOpeningHandler : IRequestHandler<EditJobOpeningCommand, Result>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IHttpContextAccessor httpContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _httpContextAccessor = httpContext;
        }

        public async Task<Result> Handle(EditJobOpeningCommand cmd, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the entity
            var jobOpening = await _jobOpeningRepository.GetByIdAsync(cmd.JobOpeningId, cancellationToken);
            if (jobOpening == null)
            {
                throw new NotFoundException("Job Opening Not Found.");
            }

            // step 2: edit entity

            // skill over ride edit
            var skillOverRides = cmd.SkillOverRides.Select(
                    selector: x => SkillOverRide.CreateForJobOpening(
                            id: x.Id ?? Guid.NewGuid(),
                            jobOpeningId: jobOpening.Id,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.JobOpening
                        )
                ).ToList();

            // interviewers edit
            var interviewers = cmd.Interviewers.Select(
                    selector: x => JobOpeningInterviewer.Create(
                            id: x.Id ?? Guid.NewGuid(),
                            jobOpeningId: jobOpening.Id,
                            userId: x.UserId,
                            role: x.Role
                        )
                ).ToList();

            // interview rounds edit
            var interviewRounds = cmd.InterviewRounds.Select(
                selector: x =>
                    {
                        var roundTemplateId = x.Id ?? Guid.NewGuid();
                        return InterviewRoundTemplate.Create(
                            id: roundTemplateId,
                            jobOpeningId: jobOpening.Id,
                            description: x.Description,
                            roundNumber: x.RoundNumber,
                            type: x.Type,
                            durationInMinutes: x.DurationInMinutes,
                            panelRequirements: x.Requirements.Select(
                                    selector: y => InterviewPanelRequirement.Create(
                                            id: y.Id ?? Guid.NewGuid(),
                                            interviewTemplateId: roundTemplateId,
                                            role: y.Role,
                                            requiredCount: y.RequirementCount
                                        )
                                ).ToList()
                        );
                    }
                ).ToList();

            // update aggregate root
            jobOpening.Update(
                updatedBy: Guid.Parse(userIdString),
                positionBatchId: cmd.PositionBatchId,
                title: cmd.Title,
                type: cmd.Type,
                description: cmd.Description,
                jobOpeningInterviewers: interviewers,
                interviewRounds: interviewRounds,
                skillOverRides: skillOverRides
            );

            // step 3: persist entity
            await _jobOpeningRepository.UpdateAysnc(jobOpening, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}