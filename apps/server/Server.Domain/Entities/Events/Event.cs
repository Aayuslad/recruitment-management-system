using Server.Core.Primitives;
using Server.Domain.Entities.Abstractions;
using Server.Domain.Enums;

namespace Server.Domain.Entities.Events
{
    public class Event : AuditableEntity, IAggregateRoot
    {
        private Event() : base(Guid.Empty, Guid.Empty) { }

        private Event(
            Guid? id,
            Guid createdBy,
            string name,
            EventType type,
            IEnumerable<EventJobOpening> jobOpenings
        ) : base(id ?? Guid.NewGuid(), createdBy)
        {
            Name = name;
            Type = type;
            EventJobOpenings = jobOpenings.ToHashSet();
        }

        public string Name { get; private set; } = null!;
        public EventType Type { get; private set; }
        public ICollection<EventJobOpening> EventJobOpenings { get; private set; } = new HashSet<EventJobOpening>();

        public static Event Create(
            Guid? id,
            Guid createdBy,
            string name,
            EventType type,
            IEnumerable<EventJobOpening> eventJobOpenings
        )
        {
            return new Event(
                id,
                createdBy,
                name,
                type,
                eventJobOpenings
            );
        }

        public void Delete(Guid deletedBy)
        {
            MarkAsDeleted(deletedBy);
        }

        public void Update(
            Guid updatedBy,
            string name,
            EventType type,
            IEnumerable<EventJobOpening> eventJobOpenings
        )
        {
            Name = name;
            Type = type;

            SyncJobOpenings(eventJobOpenings);

            MarkAsUpdated(updatedBy);
        }

        private void SyncJobOpenings(IEnumerable<EventJobOpening> newItems)
        {
            if (newItems is null) return;

            // remove
            foreach (var existing in EventJobOpenings.ToList())
            {
                if (!newItems.Any(x => x.JobOpeningId == existing.JobOpeningId))
                    EventJobOpenings.Remove(existing);
            }

            // add
            foreach (var item in newItems)
            {
                if (!EventJobOpenings.Any(x => x.JobOpeningId == item.JobOpeningId))
                    EventJobOpenings.Add(item);
            }
        }
    }
}