using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedback");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Stage)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.Comment)
                .IsRequired(false);

            builder.Property(x => x.Rating)
                .IsRequired();

            builder.HasOne(x => x.JobApplication)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Interview)
                .WithMany(x => x.Feedbacks)
                .HasForeignKey(x => x.InterviewId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.GivenByUser)
                .WithMany()
                .HasForeignKey(x => x.GivenById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}