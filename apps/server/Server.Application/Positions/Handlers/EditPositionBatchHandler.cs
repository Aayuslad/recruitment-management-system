using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.Positions.Handlers
{
    internal class EditPositionBatchHandler : IRequestHandler<EditPositionBatchCommand, Result>
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

            // step 2: update entity

            // new updated skill overrides list
            var overRides = command.SkillOverRides?.Select(
                    selector: x => SkillOverRide.CreateForPosition(
                            id: x.Id ?? Guid.NewGuid(),
                            positionBatchId: positionBatch.Id,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            minExperienceYears: x.MinExperienceYears,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.Position
                        )
                ).ToList() ?? [];

            // new updated revievers list
            var revievers = command.Reviewers?.Select(
                    selector: x => PositionBatchReviewers.Create(
                            positionBatchId: positionBatch.Id,
                            reviewerUserId: x.ReviewerUserId
                        )
                ).ToList() ?? [];

            // update root entity
            positionBatch.Update(
                updatedBy: Guid.Parse(userIdString),
                designationId: command.DesignationId,
                description: command.Description,
                jobLocation: command.JobLocation,
                minCTC: command.MinCTC,
                maxCTC: command.MaxCTC,
                newReviewers: revievers,
                newOverRides: overRides
            );

            // step 3: persist edits
            await _positionBatchRepository.UpdateAsync(positionBatch, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}