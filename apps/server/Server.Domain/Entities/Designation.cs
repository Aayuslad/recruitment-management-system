using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Designation : AuditableEntity, IAggregateRoot
    {
        private Designation() : base(Guid.Empty, Guid.Empty) { }

        private Designation(
            Guid? id,
            string name,
            string description,
            IEnumerable<DesignationSkill> skills,
            Guid createdBy
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            Name = name;
            Description = description;

            DesignationSkills = skills.ToHashSet();
        }

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public ICollection<DesignationSkill> DesignationSkills { get; private set; } =
            new HashSet<DesignationSkill>();

        public static Designation Create(
            Guid? id,
            string name,
            string description,
            Guid createdBy,
            IEnumerable<DesignationSkill> skills
        )
        {
            return new Designation(
                id,
                name,
                description,
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
            string description,
            IEnumerable<DesignationSkill> newSkills,
            Guid updatedBy
        )
        {
            Name = name;
            Description = description;

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