using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class SkillOverRideConfiguration : IEntityTypeConfiguration<SkillOverRide>
    {
        public void Configure(EntityTypeBuilder<SkillOverRide> builder)
        {
            builder.ToTable("SkillOverRide");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.PositionBatch)
                .WithMany(x => x.SkillOverRides)
                .IsRequired(false)
                .HasForeignKey(x => x.PositionBatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.JobOpening)
                .WithMany(x => x.SkillOverRides)
                .IsRequired(false)
                .HasForeignKey(x => x.JobOpeningId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Comments)
                .IsRequired(false);

            builder.Property(x => x.MinExperienceYears)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.ActionType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.SourceType)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}