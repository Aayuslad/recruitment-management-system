using Server.Core.Entities;
using Server.Domain.Entities.JobOpenings;
using Server.Domain.Entities.Positions;
using Server.Domain.Entities.Skills;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class SkillOverRide : BaseEntity<Guid>
    {
        private SkillOverRide() : base(Guid.Empty) { }

        private SkillOverRide(
            Guid? id,
            Guid? positionBatchId,
            Guid? jobOpeningId,
            Guid skillId,
            string? comments,
            SkillType type,
            SkillActionType actionType,
            SkillSourceType sourceType
        ) : base(id ?? Guid.NewGuid())
        {
            PositionBatchId = positionBatchId;
            JobOpeningId = jobOpeningId;
            SkillId = skillId;
            Comments = comments;
            Type = type;
            ActionType = actionType;
            SourceType = sourceType;
        }

        public Guid? PositionBatchId { get; private set; }
        public Guid? JobOpeningId { get; private set; }
        public Guid SkillId { get; private set; }
        public string? Comments { get; private set; } = string.Empty;
        public SkillType Type { get; private set; }
        public SkillActionType ActionType { get; private set; }
        public SkillSourceType SourceType { get; private set; }
        public PositionBatch? PositionBatch { get; private set; }
        public JobOpening? JobOpening { get; private set; }
        public Skill Skill { get; private set; } = null!;

        public static SkillOverRide CreateForJobOpening(
            Guid? id,
            Guid jobOpeningId,
            Guid skillId,
            string? comments,
            SkillType type,
            SkillActionType actionType,
            SkillSourceType sourceType
        )
        {
            return new SkillOverRide(
                id,
                null,
                jobOpeningId,
                skillId,
                comments,
                type,
                actionType,
                sourceType
            );
        }

        public static SkillOverRide CreateForPosition(
            Guid? id,
            Guid positionBatchId,
            Guid skillId,
            string? comments,
            SkillType type,
            SkillActionType actionType,
            SkillSourceType sourceType
        )
        {
            return new SkillOverRide(
                id,
                positionBatchId,
                null,
                skillId,
                comments,
                type,
                actionType,
                sourceType
            );
        }

        public void Update(
            string? comments,
            SkillType type,
            SkillActionType actionType
        )
        {
            Comments = comments;
            Type = type;
            ActionType = actionType;
        }
    }
}