using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Positions.Queries;
using Server.Application.Aggregates.Positions.Queries.DTOs;
using Server.Application.Aggregates.Positions.Queries.DTOs.PositionBatchDTOs;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Positions.Handlers
{
    internal class GetPositionBatchHandler : IRequestHandler<GetPositionBatchQuery, Result<PositionBatchDetailDTO>>
    {
        private readonly IPositionBatchRepository _batchRepository;

        public GetPositionBatchHandler(IPositionBatchRepository batchRepository)
        {
            _batchRepository = batchRepository;
        }

        public async Task<Result<PositionBatchDetailDTO>> Handle(GetPositionBatchQuery query, CancellationToken cancellationToken)
        {
            // step 1: fetch positinoBatch
            var batch = await _batchRepository.GetByIdAsync(query.BatchId, cancellationToken);
            if (batch == null)
            {
                throw new NotFoundExeption("Position Batch Not Found.");
            }

            // step 2: make dto
            // designation skills (the source of skills)
            var skills = batch.Designation.DesignationSkills.Select(ds =>
            {
                return new SkillDetailDTO
                {
                    SkillId = ds.SkillId,
                    SkillName = ds.Skill.Name,
                    SkillType = ds.SkillType,
                    MinExperienceYears = ds.MinExperienceYears,
                };
            }).ToList();

            // overrides for curret position
            foreach (var overRide in batch.SkillOverRides)
            {
                switch (overRide.ActionType)
                {
                    case SkillActionType.Add:
                        skills.Add(new SkillDetailDTO
                        {
                            SkillId = overRide.SkillId,
                            SkillName = overRide.Skill.Name,
                            MinExperienceYears = overRide.MinExperienceYears,
                            SkillType = overRide.Type,
                        });
                        break;

                    case SkillActionType.Update:
                        var skill = skills.FirstOrDefault(x => x.SkillId == overRide.SkillId);
                        if (skill != null)
                        {
                            skill.MinExperienceYears = overRide.MinExperienceYears;
                            skill.SkillType = overRide.Type;
                        }
                        break;

                    case SkillActionType.Remove:
                        skills.RemoveAll(x => x.SkillId == overRide.SkillId);
                        break;
                }
            }

            var positionBatchDetailDto = new PositionBatchDetailDTO
            {
                BatchId = batch.Id,
                Description = batch.Description,
                DesignationId = batch.DesignationId,
                DesignationName = batch.Designation.Name,
                JobLocation = batch.JobLocation,
                MinCTC = batch.MinCTC,
                MaxCTC = batch.MaxCTC,
                PositionsCount = batch.Positions.Count,
                ClosedPositionsCount = batch.Positions.Count(x => x.Status == PositionStatus.Closed),
                PositionsOnHoldCount = batch.Positions.Count(x => x.Status == PositionStatus.OnHold),
                CreatedBy = batch.CreatedBy,
                CreatedByUserName = batch.CreatedByUser?.Auth.UserName,
                CreatedAt = batch.CreatedAt,
                Reviewers = batch.Reviewers.Select(reviewer =>
                {
                    return new ReviewersDetailDTO
                    {
                        ReviewerUserId = reviewer.ReviewerId,
                        ReviewerUserName = reviewer.ReviewerUser.Auth.UserName,
                        ReviewerUserEmail = reviewer.ReviewerUser.Auth.Email.ToString(),
                    };
                }).ToList(),
                Skills = skills,
                SkillOverRides = batch.SkillOverRides.Select(x =>
                {
                    return new SkillOverRideDetailDTO
                    {
                        Id = x.Id,
                        SkillId = x.SkillId,
                        Comments = x.Comments,
                        MinExperienceYears = x.MinExperienceYears,
                        Type = x.Type,
                        ActionType = x.ActionType,
                    };
                }).ToList()
            };

            // step 3: return result
            return Result<PositionBatchDetailDTO>.Success(positionBatchDetailDto);
        }
    }
}