using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobOpeningInterviewRoundTemplateConfiguration : IEntityTypeConfiguration<JobOpeningInterviewRoundTemplate>
    {
        public void Configure(EntityTypeBuilder<JobOpeningInterviewRoundTemplate> builder)
        {
            builder.ToTable("JobOpeningInterviewRoundTemplate");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne<JobOpening>(x => x.JobOpening)
                .WithMany(x => x.InterviewRounds)
                .HasForeignKey(x => x.JobOpeningId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Description)
                .IsRequired(false);

            builder.Property(x => x.RoundNumber)
                .IsRequired();

            builder.Property(x => x.DurationInMinutes)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.HasIndex(x => new { x.JobOpeningId, x.RoundNumber })
                .IsUnique();
        }
    }
}