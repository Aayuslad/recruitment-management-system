using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Entities.Positions;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
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
                throw new UnAuthorisedExeption();
            }

            // step 1: fetch positinoBatch
            var positionBatch = await _positionBatchRepository.GetByIdAsync(command.PositionBatchId, cancellationToken);
            if (positionBatch == null)
            {
                throw new NotFoundExeption("Position Not Found.");
            }

            // step 2: update entity

            // new updated skill overrides list
            var overRides = command.SkillOverRides?.Select(
                    selector: x => SkillOverRide.CreateForPosition(
                            id: x.Id ?? Guid.NewGuid(),
                            positionBatchId: positionBatch.Id,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.Position
                        )
                ).ToList() ?? [];

            // new updated revievers list
            var revievers = command.Reviewers?.Select(
                    selector: x => PositionBatchReviewer.Create(
                            positionBatchId: positionBatch.Id,
                            reviewerId: x.ReviewerUserId
                        )
                ).ToList() ?? [];

            // update root entity
            positionBatch.Update(
                updatedBy: Guid.Parse(userIdString),
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