using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.JobApplications;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobApplicationStatusMoveHistoryConfiguration : IEntityTypeConfiguration<JobApplicationStatusMoveHistory>
    {
        public void Configure(EntityTypeBuilder<JobApplicationStatusMoveHistory> builder)
        {
            builder.ToTable("JobApplicationStatusMoveHistory");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.StatusMovedTo)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.MovedAt)
                .IsRequired();

            builder.Property(x => x.Comment)
                .IsRequired(false);

            builder.HasOne(x => x.JobApplication)
                .WithMany(x => x.StatusMoveHistories)
                .HasForeignKey(x => x.JobApplicationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.MovedByUser)
                .WithMany()
                .HasForeignKey(x => x.MovedById)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}