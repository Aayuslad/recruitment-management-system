using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Events;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class EventJobOpeningConfiguration : IEntityTypeConfiguration<EventJobOpening>
    {
        public void Configure(EntityTypeBuilder<EventJobOpening> builder)
        {
            builder.ToTable("EventJobApplication");

            builder.HasKey(e => new { e.EventId, e.JobOpeningId });

            builder.HasOne(x => x.Event)
                .WithMany(x => x.EventJobOpenings)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.JobOpening)
                .WithMany()
                .HasForeignKey(x => x.JobOpeningId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}