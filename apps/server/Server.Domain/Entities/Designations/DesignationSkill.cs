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
            SkillType skillType,
            float? minExperienceYears
        )
        {
            DesignationId = designationId;
            SkillId = skillId;
            SkillType = skillType;
            MinExperienceYears = minExperienceYears;
        }

        public Guid DesignationId { get; private set; }
        public Guid SkillId { get; private set; }
        public SkillType SkillType { get; private set; }
        public float? MinExperienceYears { get; private set; }
        public Skill Skill { get; private set; } = default!;
        public Designation Designation { get; private set; } = default!;

        public static DesignationSkill Create(
            Guid designationId,
            Guid skillId,
            SkillType skillType,
            float? minExperienceYears
        )
        {
            return new DesignationSkill(
                designationId,
                skillId,
                skillType,
                minExperienceYears
            );
        }

        public void Update(SkillType skillType, float? minExperienceYears)
        {
            SkillType = skillType;
            MinExperienceYears = minExperienceYears;
        }
    }
}