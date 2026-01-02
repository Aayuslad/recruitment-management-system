using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Candidates;
using Server.Domain.ValueObjects;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class CandidateConfiguration : AuditableEntityConfiguration<Candidate>
    {
        public override void Configure(EntityTypeBuilder<Candidate> builder)
        {
            base.Configure(builder);

            builder.ToTable("Candidate");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(u => u.ContactNumber)
                .HasConversion(
                    contactNumberVO => contactNumberVO.ToString(),
                    contactNumber => ContactNumber.Create(contactNumber)!
                )
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("ContactNumber");
            //builder.HasIndex(u => u.ContactNumber)
            //    .IsUnique();

            builder.Property(a => a.Email)
                .HasConversion(
                    emailVO => emailVO.ToString(),
                    email => Email.Create(email)!
                )
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("Email");
            //builder.HasIndex(a => a.Email).IsUnique();

            builder.Property(x => x.FirstName)
                .IsRequired();

            builder.Property(x => x.MiddleName)
                .IsRequired(false);

            builder.Property(x => x.LastName)
                .IsRequired();

            builder.Property(x => x.Gender)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.Dob)
                .IsRequired();

            builder.Property(x => x.CollegeName)
                .IsRequired();

            builder.Property(x => x.ResumeUrl)
                .IsRequired();

            builder.Property(x => x.IsBgVerificationCompleted)
                .IsRequired();

            builder.HasOne(x => x.BgVerifiedByUser)
                .WithMany()
                .HasForeignKey(x => x.BgVerifiedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}