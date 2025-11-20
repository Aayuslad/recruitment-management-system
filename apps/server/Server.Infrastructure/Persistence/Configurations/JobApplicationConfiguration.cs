using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class JobApplicationConfiguration : AuditableEntityConfiguration<JobApplication>
    {
        public override void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            base.Configure(builder);

            builder.ToTable("JobApplication");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.AppliedAt)
                .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.Candidate)
                .WithMany()
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.JobOpening)
                .WithMany()
                .HasForeignKey(x => x.JobOpeningId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.CandidateId, x.JobOpeningId })
                .IsUnique();
        }
    }
}