using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Positions.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Entities.Positions;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class EditPositionBatchHandler : IRequestHandler<EditPositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IUserContext _userContext;

        public EditPositionBatchHandler(IPositionBatchRepository positionBatchRepository, IUserContext userContext)
        {
            _positionBatchRepository = positionBatchRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditPositionBatchCommand request, CancellationToken cancellationToken)
        {
            // step 1: fetch positinoBatch
            var positionBatch = await _positionBatchRepository.GetByIdAsync(request.PositionBatchId, cancellationToken);
            if (positionBatch == null)
            {
                throw new NotFoundException("Position Not Found.");
            }

            // step 2: update entity

            // new updated skill overrides list
            var overRides = request.SkillOverRides?.Select(
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
            var revievers = request.Reviewers?.Select(
                    selector: x => PositionBatchReviewer.Create(
                            positionBatchId: positionBatch.Id,
                            reviewerId: x.ReviewerUserId
                        )
                ).ToList() ?? [];

            // update root entity
            positionBatch.Update(
                updatedBy: _userContext.UserId,
                description: request.Description,
                jobLocation: request.JobLocation,
                minCTC: request.MinCTC,
                maxCTC: request.MaxCTC,
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