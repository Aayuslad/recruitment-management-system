using Server.Domain.Entities.Skills;
using Server.Domain.Enums;

namespace Server.Domain.Entities.Designations
{
    public class DesignationSkill
    {
        private DesignationSkill() { }

        private DesignationSkill(
            Guid designationId,
            Guid skillId,
            SkillType skillType
        )
        {
            DesignationId = designationId;
            SkillId = skillId;
            SkillType = skillType;
        }

        public Guid DesignationId { get; private set; }
        public Guid SkillId { get; private set; }
        public SkillType SkillType { get; private set; }
        public Skill Skill { get; private set; } = default!;
        public Designation Designation { get; private set; } = default!;

        public static DesignationSkill Create(
            Guid designationId,
            Guid skillId,
            SkillType skillType
        )
        {
            return new DesignationSkill(
                designationId,
                skillId,
                skillType
            );
        }

        public void Update(SkillType skillType)
        {
            SkillType = skillType;
        }
    }
}