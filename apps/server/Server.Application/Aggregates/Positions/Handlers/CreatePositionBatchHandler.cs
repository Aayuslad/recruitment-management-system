using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Positions.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Entities.Positions;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class CreatePositionBatchHandler : IRequestHandler<CreatePositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IUserContext _userContext;

        public CreatePositionBatchHandler(IPositionBatchRepository positionBatchRepository, IUserContext userContext)
        {
            _positionBatchRepository = positionBatchRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(CreatePositionBatchCommand request, CancellationToken cancellationToken)
        {
            // step 1: create entity
            var newPositionBatchId = Guid.NewGuid();

            // create skil over rides entity list
            var overrides = request.SkillOverRides?.Select(
                    selector: x => SkillOverRide.CreateForPosition(
                            id: null,
                            positionBatchId: newPositionBatchId,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.Position
                        )
                ).ToList() ?? [];

            // create reviewers list
            var reviewers = request.Reviewers?.Select(
                selector: reviewer => PositionBatchReviewer.Create(
                            positionBatchId: newPositionBatchId,
                            reviewerId: reviewer.ReviewerUserId
                        )).ToList() ?? [];

            // create needed positions list
            var positions = new List<Position>();
            for (int i = 0; i < request.NumberOfPositions; i++)
            {
                var position = Position.Create(newPositionBatchId);
                positions.Add(position);
            }

            // create root entity
            var positionBatch = PositionBatch.Create(
                   id: newPositionBatchId,
                   createdBy: _userContext.UserId,
                   description: request.Description,
                   designationId: request.DesignationId,
                   jobLocation: request.JobLocation,
                   maxCTC: request.MaxCTC,
                   minCTC: request.MinCTC,
                   positions: positions,
                   reviewers: reviewers,
                   overRides: overrides
               );

            // step 2: persist position batch
            await _positionBatchRepository.AddAsync(positionBatch, cancellationToken);

            // step 3: return result
            return Result.Success();
        }
    }
}