using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Positions;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class PositionBatchReviewersConfiguration : IEntityTypeConfiguration<PositionBatchReviewer>
    {
        public void Configure(EntityTypeBuilder<PositionBatchReviewer> builder)
        {
            builder.ToTable("PositionBatchReviewer");

            builder.HasKey(x => new { x.PositionBatchId, x.ReviewerId });

            builder.HasOne(x => x.PositionBatch)
                .WithMany(x => x.Reviewers)
                .IsRequired()
                .HasForeignKey(x => x.PositionBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReviewerUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}