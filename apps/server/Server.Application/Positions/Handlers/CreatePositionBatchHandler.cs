using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.Positions.Handlers
{
    internal class CreatePositionBatchHandler : IRequestHandler<CreatePositionBatchCommand, Result>
    {
        private readonly IPositionBatchRepository _positionBatchRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public CreatePositionBatchHandler(IPositionBatchRepository positionBatchRepository, IHttpContextAccessor contextAccessor)
        {
            _positionBatchRepository = positionBatchRepository;
            _contextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreatePositionBatchCommand command, CancellationToken cancellationToken)
        {
            var userIdString = _contextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: create entity
            var newPositionBatchId = Guid.NewGuid();

            // create skil over rides entity list
            var overrides = command.SkillOverRides?.Select(
                    selector: x => SkillOverRide.CreateForPosition(
                            id: null,
                            positionBatchId: newPositionBatchId,
                            skillId: x.SkillId,
                            comments: x.Comments,
                            minExperienceYears: x.MinExperienceYears,
                            type: x.Type,
                            actionType: x.ActionType,
                            sourceType: SkillSourceType.Position
                        )
                ).ToList() ?? [];

            // create reviewers list
            var reviewers = command.Reviewers?.Select(
                selector: reviewer => PositionBatchReviewer.Create(
                            positionBatchId: newPositionBatchId,
                            reviewerId: reviewer.ReviewerUserId
                        )).ToList() ?? [];

            // create needed positions list
            var positions = new List<Position>();
            for (int i = 0; i < command.NumberOfPositions; i++)
            {
                var position = Position.Create(newPositionBatchId);
                positions.Add(position);
            }

            // create root entity
            var positionBatch = PositionBatch.Create(
                   id: newPositionBatchId,
                   createdBy: Guid.Parse(userIdString),
                   description: command.Description,
                   designationId: command.DesignationId,
                   jobLocation: command.JobLocation,
                   maxCTC: command.MaxCTC,
                   minCTC: command.MinCTC,
                   positions: positions,
                   reviewers: reviewers,
                   overRides: overrides
               );

            // step 5: persist position batch
            await _positionBatchRepository.AddAsync(positionBatch, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}