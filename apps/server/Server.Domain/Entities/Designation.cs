using Server.Core.Entities;
using Server.Core.Primitives;

namespace Server.Domain.Entities
{
    public class Designation : AuditableEntity, IAggregateRoot
    {
        //private readonly List<DesignationSkill> _designationSkills = new();

        private Designation() : base(Guid.Empty, Guid.Empty) { }

        private Designation(Guid id, string name, string description, Guid createdBy)
            : base(id, createdBy)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public ICollection<DesignationSkill> DesignationSkills { get; private set; } = new List<DesignationSkill>();

        public static Designation Create(string name, string description, Guid createdBy)
        {
            return new Designation(Guid.NewGuid(), name, description, createdBy);
        }

        public void AddSkill(DesignationSkill designationSkill)
        {
            DesignationSkills.Add(designationSkill);
        }

        public void RemoveSkill(DesignationSkill designationSkill)
        {
            DesignationSkills.Remove(designationSkill);
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void Update(string name, string description, List<DesignationSkill> skills, Guid updatedBy)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("Description cannot be null or empty", nameof(description));

            Name = name;
            Description = description;
            DesignationSkills.Clear();
            foreach (var skill in skills)
            {
                DesignationSkills.Add(skill);
            }
            MarkAsUpdated(updatedBy);
        }
    }
}