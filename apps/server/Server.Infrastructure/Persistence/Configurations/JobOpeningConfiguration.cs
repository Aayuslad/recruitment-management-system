using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobOpeningConfiguration : AuditableEntityConfiguration<JobOpening>
    {
        public override void Configure(EntityTypeBuilder<JobOpening> builder)
        {
            base.Configure(builder);

            builder.ToTable("JobOpening");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired();

            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.Property(x => x.Type)
                .IsRequired();

            builder.HasOne(x => x.PositionBatch)
                .WithMany()
                .HasForeignKey(x => x.PositionBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}