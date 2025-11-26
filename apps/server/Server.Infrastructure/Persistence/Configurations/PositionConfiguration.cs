using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Positions;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Position");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.PositionBatch)
                .WithMany(x => x.Positions)
                .IsRequired()
                .HasForeignKey(x => x.BatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.Candidate)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(x => x.ClosedByCandidate)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.ClosureReason)
                .IsRequired(false);
        }
    }
}