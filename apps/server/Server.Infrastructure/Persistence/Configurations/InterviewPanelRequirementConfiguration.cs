using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.JobOpenings;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class InterviewPanelRequirementConfiguration : IEntityTypeConfiguration<InterviewPanelRequirement>
    {
        public void Configure(EntityTypeBuilder<InterviewPanelRequirement> builder)
        {
            builder.ToTable("InterviewPanelRequirement");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne<InterviewRoundTemplate>(x => x.InterviewRoundTemplate)
                .WithMany(x => x.PanelRequirements)
                .HasForeignKey(x => x.InterviewTemplateId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.RequiredCount)
                .IsRequired();

            builder.HasIndex(x => new { x.InterviewTemplateId, x.Role })
                .IsUnique();
        }
    }
}