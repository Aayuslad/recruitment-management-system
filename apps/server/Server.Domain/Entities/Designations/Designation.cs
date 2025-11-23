using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;

namespace Server.Domain.Entities.Designations
{
    public class Designation : AuditableEntity, IAggregateRoot
    {
        private Designation() : base(Guid.Empty, Guid.Empty) { }

        private Designation(
            Guid? id,
            string name,
            IEnumerable<DesignationSkill> skills,
            Guid createdBy
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            Name = name;
            DesignationSkills = skills.ToHashSet();
        }

        public string Name { get; private set; } = default!;
        public ICollection<DesignationSkill> DesignationSkills { get; private set; } =
            new HashSet<DesignationSkill>();

        public static Designation Create(
            Guid? id,
            string name,
            Guid createdBy,
            IEnumerable<DesignationSkill> skills
        )
        {
            return new Designation(
                id,
                name,
                skills,
                createdBy
            );
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void Update(
            string name,
            IEnumerable<DesignationSkill> newSkills,
            Guid updatedBy
        )
        {
            Name = name;

            SyncDesignatioSkills(newSkills);

            MarkAsUpdated(updatedBy);
        }

        private void SyncDesignatioSkills(IEnumerable<DesignationSkill> newDesignationSkills)
        {
            if (newDesignationSkills is null) return;

            // remove removed ones
            foreach (var dSkill in DesignationSkills.ToList())
            {
                if (!newDesignationSkills.Any(x => x.SkillId == dSkill.SkillId))
                    DesignationSkills.Remove(dSkill);
            }

            foreach (var newSkill in newDesignationSkills)
            {
                var toUpdate = DesignationSkills.FirstOrDefault(x => x.SkillId == newSkill.SkillId);
                toUpdate?.Update(
                    newSkill.SkillType,
                    newSkill.MinExperienceYears
                );
            }

            // add added ones
            foreach (var newDSkill in newDesignationSkills)
            {
                if (!DesignationSkills.Any(x => x.SkillId == newDSkill.SkillId))
                    DesignationSkills.Add(newDSkill);
            }
        }
    }
}