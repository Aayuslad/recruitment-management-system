
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobOpenings.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.JobOpenings.Handlers
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

        public async Task<Result> Handle(EditJobOpeningCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the entity
            var jobOpening = await _jobOpeningRepository.GetByIdAsync(request.JobOpeningId, cancellationToken);
            if (jobOpening == null)
            {
                return Result.Failure("Job opening not found", 404);
            }

            // step 2: edit entity

            // skill over ride edit
            var skillOverRides = request.SkillOverRides.Select(
                    selector: x => SkillOverRide.CreateForJobOpening(
                            id: x.Id ?? Guid.NewGuid(),
                            jobOpeningId: jobOpening.Id,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            minExperienceYears: x.MinExperienceYears,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.JobOpening
                        )
                ).ToList();

            // interviewers edit
            var interviewers = request.Interviewers.Select(
                    selector: x => JobOpeningInterviewer.Create(
                            id: x.Id ?? Guid.NewGuid(),
                            jobOpeningId: jobOpening.Id,
                            userId: x.UserId,
                            role: x.Role
                        )
                ).ToList();

            // interview rounds edit
            var interviewRounds = request.InterviewRounds.Select(
                selector: x =>
                    {
                        var roundTemplateId = x.Id ?? Guid.NewGuid();
                        return JobOpeningInterviewRoundTemplate.Create(
                            id: roundTemplateId,
                            jobOpeningId: jobOpening.Id,
                            description: x.Description,
                            roundNumber: x.RoundNumber,
                            type: x.Type,
                            durationInMinutes: x.DurationInMinutes,
                            panelRequirements: x.Requirements.Select(
                                    selector: y => JobOpeningInterviewPanelRequirement.Create(
                                            id: y.Id ?? Guid.NewGuid(),
                                            jobOpeningInterviewTemplateId: roundTemplateId,
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
                positionBatchId: request.PositionBatchId,
                title: request.Title,
                type: request.Type,
                description: request.Description,
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