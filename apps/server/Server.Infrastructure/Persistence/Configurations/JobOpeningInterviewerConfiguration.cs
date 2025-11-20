using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobOpeningInterviewerConfiguration : IEntityTypeConfiguration<JobOpeningInterviewer>
    {
        public void Configure(EntityTypeBuilder<JobOpeningInterviewer> builder)
        {
            builder.ToTable("JobOpeningInterviewer");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.HasOne<JobOpening>(x => x.JobOpening)
                .WithMany(x => x.JobOpeningInterviewers)
                .HasForeignKey(x => x.JobOpeningId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<User>(x => x.InterviewerUser)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.HasIndex(x => new { x.JobOpeningId, x.UserId, x.Role })
                .IsUnique();
        }
    }
}