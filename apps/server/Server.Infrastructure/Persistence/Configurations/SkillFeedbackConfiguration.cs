using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class SkillFeedbackConfiguration : IEntityTypeConfiguration<SkillFeedback>
    {
        public void Configure(EntityTypeBuilder<SkillFeedback> builder)
        {
            builder.ToTable("SkillFeedback");

            builder.HasKey(x => new { x.FeedbackId, x.SkillId });

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.Property(x => x.AssessedExpYears)
                .IsRequired(false);

            builder.HasOne(x => x.Feedback)
                .WithMany(x => x.SkillFeedbacks)
                .HasForeignKey(x => x.FeedbackId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(x => x.SkillId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
