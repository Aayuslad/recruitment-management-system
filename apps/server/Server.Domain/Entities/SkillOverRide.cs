using Server.Core.Entities;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class SkillOverRide : BaseEntity<Guid>
    {
        private SkillOverRide() : base(Guid.Empty) { }

        private SkillOverRide(
            Guid positionBatchId,
            Guid skillId,
            string? comments,
            float minExperienceYears,
            SkillType type,
            SkillActionType actionType,
            SkillSourceType sourceType
        ) : base(Guid.NewGuid())
        {
            PositionBatchId = positionBatchId;
            SkillId = skillId;
            Comments = comments;
            MinExperienceYears = minExperienceYears;
            Type = type;
            ActionType = actionType;
            SourceType = sourceType;
        }

        public Guid PositionBatchId { get; private set; }
        // add job opening id later
        public Guid SkillId { get; private set; }
        public string? Comments { get; private set; } = string.Empty;
        public float MinExperienceYears { get; private set; }
        public SkillType Type { get; private set; }
        public SkillActionType ActionType { get; private set; }
        public SkillSourceType SourceType { get; private set; }
        public PositionBatch PositionBatch { get; private set; } = null!;
        public Skill Skill { get; private set; } = null!;

        public static SkillOverRide Create(
                Guid positionBatchId,
                Guid skillId,
                string? comments,
                float minExperienceYears,
                SkillType type,
                SkillActionType actionType,
                SkillSourceType sourceType
            )
        {
            return new SkillOverRide(
                    positionBatchId,
                    skillId,
                    comments,
                    minExperienceYears,
                    type,
                    actionType,
                    sourceType
                );
        }

        public void Update(
            string? comments,
            float minExperienceYears,
            SkillType type,
            SkillActionType actionType
        )
        {
            Comments = comments;
            MinExperienceYears = minExperienceYears;
            Type = type;
            ActionType = actionType;
        }
    }
}