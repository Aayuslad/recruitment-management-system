using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
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

            builder.OwnsOne(u => u.ContactNumber, cn =>
            {
                cn.Property(c => c.Number)
                  .HasColumnName("ContactNumber")
                  .HasMaxLength(20);
            });

            builder.Property(u => u.Gender)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(u => u.Dob)
                .IsRequired();

            builder.Property(u => u.DeletedAt);

            // FK: User → Auth (1:1)
            builder.HasOne<Auth>()
                .WithOne()
                .HasForeignKey<User>(u => u.AuthId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}