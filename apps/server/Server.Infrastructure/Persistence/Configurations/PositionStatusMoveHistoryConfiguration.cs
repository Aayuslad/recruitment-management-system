using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class PositionStatusMoveHistoryConfiguration : IEntityTypeConfiguration<PositionStatusMoveHistory>
    {
        public void Configure(EntityTypeBuilder<PositionStatusMoveHistory> builder)
        {
            builder.ToTable("PositionStatusMoveHistory");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne(x => x.Position)
                .WithMany(x => x.StatusMoveHistories)
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.MovedTo)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.Comments)
                .IsRequired(false);

            builder.Property(x => x.MovedAt)
                .IsRequired();

            builder.HasOne(x => x.MovedByUser)
                .WithMany()
                .HasForeignKey(x => x.MovedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}