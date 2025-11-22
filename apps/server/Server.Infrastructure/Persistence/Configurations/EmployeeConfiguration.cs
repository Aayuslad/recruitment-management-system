using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(u => u.ContactNumber)
                .HasConversion(
                    contactNumberVO => contactNumberVO.ToString(),
                    contactNumber => ContactNumber.Create(contactNumber).Value!
                )
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("ContactNumber");
            //builder.HasIndex(u => u.ContactNumber)
            //    .IsUnique();

            builder.Property(a => a.Email)
                .HasConversion(
                    emailVO => emailVO.ToString(),
                    email => Email.Create(email).Value!
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

            builder.Property(x => x.Dob)
                .IsRequired();

            builder.HasOne(x => x.Designation)
                .WithMany()
                .HasForeignKey(x => x.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}