using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class PositionBatchConfiguration : AuditableEntityConfiguration<PositionBatch>
    {
        public override void Configure(EntityTypeBuilder<PositionBatch> builder)
        {
            base.Configure(builder);

            builder.ToTable("PositionBatch");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.HasOne(x => x.Designation)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.JobLocation)
                .IsRequired();

            builder.Property(x => x.MinCTC)
                .IsRequired();

            builder.Property(x => x.MaxCTC)
                .IsRequired();
        }
    }
}