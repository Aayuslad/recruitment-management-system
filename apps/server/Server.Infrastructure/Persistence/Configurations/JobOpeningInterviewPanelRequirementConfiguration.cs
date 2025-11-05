using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobOpeningInterviewPanelRequirementConfiguration : IEntityTypeConfiguration<JobOpeningInterviewPanelRequirement>
    {
        public void Configure(EntityTypeBuilder<JobOpeningInterviewPanelRequirement> builder)
        {
            builder.ToTable("JobOpeningInterviewPanelRequirement");

            builder.HasKey(x => x.Id);

            builder.HasOne<JobOpeningInterviewRoundTemplate>(x => x.InterviewRoundTemplate)
                .WithMany(x => x.PanelRequirements)
                .HasForeignKey(x => x.JobOpeningInterviewTemplateId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.RequiredCount)
                .IsRequired();

            builder.HasIndex(x => new { x.JobOpeningInterviewTemplateId, x.Role })
                .IsUnique();
        }
    }
}