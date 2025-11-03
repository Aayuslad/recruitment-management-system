using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class UserConfiguration : AuditableEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("User");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.AuthId)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.MiddleName)
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Status)
                .HasConversion<string>()   // store enum as string
                .IsRequired();

            builder.Property(u => u.ContactNumber)
                .HasConversion(
                    contactNumberVO => contactNumberVO.ToString(),
                    contactNumber => ContactNumber.Create(contactNumber).Value!
                )
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("ContactNumber");
            builder.HasIndex(u => u.ContactNumber)
                .IsUnique();

            builder.Property(u => u.IsContactNumberVerified)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(u => u.Gender)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.Dob)
                .IsRequired();

            // FK: User → Auth (1:1)
            builder.HasOne(x => x.Auth)
                .WithOne()
                .HasForeignKey<User>(u => u.AuthId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}