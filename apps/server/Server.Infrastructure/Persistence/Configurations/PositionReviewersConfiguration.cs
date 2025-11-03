using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class PositionBatchReviewersConfiguration : IEntityTypeConfiguration<PositionBatchReviewers>
    {
        public void Configure(EntityTypeBuilder<PositionBatchReviewers> builder)
        {
            builder.ToTable("PositionBatchReviewer");

            builder.HasKey(x => new { x.PositionBatchId, x.ReviewerUserId });

            builder.HasOne(x => x.PositionBatch)
                .WithMany(x => x.PositionBatchReviewers)
                .IsRequired()
                .HasForeignKey(x => x.PositionBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ReviewerUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ReviewerUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}