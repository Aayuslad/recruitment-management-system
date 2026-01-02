
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
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
        private readonly IUserContext _userContext;

        public EditJobOpeningHandler(IJobOpeningRepository jobOpeningRepository, IUserContext userContext)
        {
            _jobOpeningRepository = jobOpeningRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditJobOpeningCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch the entity
            var jobOpening = await _jobOpeningRepository.GetByIdAsync(request.JobOpeningId, cancellationToken);
            if (jobOpening == null)
            {
                throw new NotFoundException("Job Opening Not Found.");
            }

            // step 2: edit entity

            // skill over ride edit
            var skillOverRides = request.SkillOverRides.Select(
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
                updatedBy: _userContext.UserId,
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