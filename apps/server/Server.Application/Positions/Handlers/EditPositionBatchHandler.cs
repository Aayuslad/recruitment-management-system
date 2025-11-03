using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.Positions.Handlers
{
    public class EditPositionBatchHandler : IRequestHandler<EditPositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public EditPositionBatchHandler(IPositionBatchRepository positionBatchRepository, IHttpContextAccessor httpContextAccessor)
        {
            _positionBatchRepository = positionBatchRepository;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<Result> Handle(EditPositionBatchCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch positinoBatch
            var positionBatch = await _positionBatchRepository.GetByIdAsync(command.PositionBatchId, cancellationToken);
            if (positionBatch == null)
            {
                return Result.Failure("Position does not exist", 404);
            }

            // step 2: update root entity
            positionBatch.Update(
                designationId: command.DesignationId,
                description: command.Description,
                jobLocation: command.JobLocation,
                minCTC: command.MinCTC,
                maxCTC: command.MaxCTC,
                updatedBy: Guid.Parse(userIdString)
            );

            // step 3: update skill overrides
            if (command.SkillOverRides != null)
            {
                foreach (var skill in command.SkillOverRides)
                {
                    var existing = positionBatch.SkillOverRides.FirstOrDefault(x => x.SkillId == skill.SkillId);
                    if (existing != null)
                    {
                        // update
                        existing.Update(
                            comments: skill.Comments,
                            minExperienceYears: skill.MinExperienceYears,
                            type: skill.Type,
                            actionType: skill.ActionType
                        );
                    }
                    else
                    {
                        // add
                        var overRide = SkillOverRide.Create(
                                positionBatchId: positionBatch.Id,
                                skillId: skill.SkillId,
                                comments: skill.Comments,
                                minExperienceYears: skill.MinExperienceYears,
                                type: skill.Type,
                                actionType: skill.ActionType,
                                sourceType: SkillSourceType.Position
                        );
                        positionBatch.AddSkillOverRides(new List<SkillOverRide> { overRide });
                    }

                    // remove
                    var toRemoveSkills = positionBatch.SkillOverRides
                        .Where(x => !command.SkillOverRides.Any(y => y.SkillId == x.SkillId))
                        .ToList();
                    positionBatch.RemoveSkillOverRides(toRemoveSkills);
                }
            }

            // step 4: update reviewers
            foreach (var reviewer in command.Reviewers ?? new())
            {
                var existing = positionBatch.PositionBatchReviewers.FirstOrDefault(x => x.ReviewerUserId == reviewer.ReviewerUserId);
                if (existing == null)
                {
                    // add
                    var newReviewer = PositionBatchReviewers.Create(
                            positionBatchId: positionBatch.Id,
                            reviewerUserId: reviewer.ReviewerUserId
                        );
                    positionBatch.AddReviewers(new List<PositionBatchReviewers> { newReviewer });
                }
            }
            // remove
            var toRemove = new List<PositionBatchReviewers>();
            if (command.Reviewers?.Count > 0)
                toRemove = positionBatch.PositionBatchReviewers
                    .Where(x => !command.Reviewers.Any(y => y.ReviewerUserId == x.ReviewerUserId))
                    .ToList();
            positionBatch.RemoveReviewers(toRemove);

            // step 5: persist edits
            await _positionBatchRepository.UpdateAsync(positionBatch, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}