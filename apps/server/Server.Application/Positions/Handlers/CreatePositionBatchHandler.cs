using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Positions.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.Enums;

namespace Server.Application.Positions.Handlers
{
    public class CreatePositionBatchHandler : IRequestHandler<CreatePositionBatchCommand, Result>
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

            // step 1: create position batch
            var positionBatch = PositionBatch.Create(
                    Guid.Parse(userIdString),
                    description: command.Description,
                    designationId: command.DesignationId,
                    jobLocation: command.JobLocation,
                    maxCTC: command.MaxCTC,
                    minCTC: command.MinCTC
                );

            // step 2: add skill overrides
            if (command.SkillOverRides?.Count > 0)
            {
                var skillOverRides = command.SkillOverRides.Select(skillOverRide =>
                    {
                        return SkillOverRide.Create(
                            positionBatchId: positionBatch.Id,
                            skillId: skillOverRide.SkillId,
                            comments: skillOverRide.Comments,
                            minExperienceYears: skillOverRide.MinExperienceYears,
                            type: skillOverRide.Type,
                            actionType: skillOverRide.ActionType,
                            sourceType: SkillSourceType.Position
                        );
                    }).ToList();

                positionBatch.AddSkillOverRides(skillOverRides);
            }

            // step 3: add reviewers for positon batch
            var reviewers = command.Reviewers?.Select(reviewer =>
                {
                    return PositionBatchReviewers.Create(
                            positionBatchId: positionBatch.Id,
                            reviewerUserId: reviewer.ReviewerUserId
                        );
                }).ToList() ?? new();
            positionBatch.AddReviewers(reviewers);

            // step 4: create needed positions and add
            var positions = new List<Position>();
            for (int i = 0; i < command.NumberOfPositions; i++)
            {
                var position = Position.Create(positionBatch.Id);
                positions.Add(position);
            }
            positionBatch.AddPositions(positions);

            // step 5: persist position batch
            await _positionBatchRepository.AddAsync(positionBatch, cancellationToken);

            // step 6: return result
            return Result.Success();
        }
    }
}