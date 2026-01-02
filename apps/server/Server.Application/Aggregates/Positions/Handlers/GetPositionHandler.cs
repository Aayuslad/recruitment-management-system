using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Queries;
using Server.Application.Aggregates.Positions.Queries.DTOs;
using Server.Application.Aggregates.Positions.Queries.DTOs.PositionDTOs;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class GetPositionHandler : IRequestHandler<GetPositionQuery, Result<PositionDetailDTO>>
    {
        private readonly IPositionRepository _positionRepository;

        public GetPositionHandler(IPositionRepository positionRepository)
        {
            _positionRepository = positionRepository;
        }

        public async Task<Result<PositionDetailDTO>> Handle(GetPositionQuery request, CancellationToken cancellationToken)
        {

            // step 1: fetch positinoBatch
            var position = await _positionRepository.GetByIdAsync(request.PositionId, cancellationToken);
            if (position == null)
            {
                throw new NotFoundException("Position Not Found.");
            }

            // step 2: make dto
            // designation skills (the source of skills)
            var skills = position.PositionBatch.Designation.DesignationSkills.Select(ds =>
            {
                return new SkillDetailDTO
                {
                    SkillId = ds.SkillId,
                    SkillName = ds.Skill.Name,
                    SkillType = ds.SkillType,
                };
            }).ToList();

            // overrides for curret position
            foreach (var overRide in position.PositionBatch.SkillOverRides)
            {
                switch (overRide.ActionType)
                {
                    case SkillActionType.Add:
                        skills.Add(new SkillDetailDTO
                        {
                            SkillId = overRide.SkillId,
                            SkillName = overRide.Skill.Name,
                            SkillType = overRide.Type,
                        });
                        break;

                    case SkillActionType.Update:
                        var skill = skills.FirstOrDefault(x => x.SkillId == overRide.SkillId);
                        if (skill != null)
                        {
                            skill.SkillType = overRide.Type;
                        }
                        break;

                    case SkillActionType.Remove:
                        skills.RemoveAll(x => x.SkillId == overRide.SkillId);
                        break;
                }
            }

            var dto = new PositionDetailDTO
            {
                PositionId = position.Id,
                BatchId = position.BatchId,
                Descripcion = position.PositionBatch.Description,
                DesignationId = position.PositionBatch.DesignationId,
                DesignationName = position.PositionBatch.Designation.Name,
                JobLocation = position.PositionBatch.JobLocation,
                MinCTC = position.PositionBatch.MinCTC,
                MaxCTC = position.PositionBatch.MaxCTC,
                Status = position.Status,
                ClosedByCandidate = position.ClosedByCandidate,
                ClosureReason = position.ClosureReason,
                Reviewers = position.PositionBatch.Reviewers.Select(reviewer =>
                {
                    return new ReviewersDetailDTO
                    {
                        ReviewerUserId = reviewer.ReviewerId,
                        ReviewerUserName = reviewer.ReviewerUser.Auth.UserName,
                        ReviewerUserEmail = reviewer.ReviewerUser.Auth.Email.ToString(),
                    };
                }).ToList(),
                Skills = skills,
                MoveHistory = position.StatusMoveHistories?.Select(x =>
                {
                    return new PositionStatusMoveHistoryDetailDTO
                    {
                        Id = x.Id,
                        PositionId = x.PositionId,
                        MovedTo = x.MovedTo,
                        Comments = x.Comments,
                        MovedAt = x.MovedAt,
                        MovedById = x.MovedById,
                        MovedByUserName = x.MovedByUser.Auth.UserName,
                    };
                }).ToList() ?? [],
            };

            // step 3: return result
            return Result<PositionDetailDTO>.Success(dto);
        }
    }
}