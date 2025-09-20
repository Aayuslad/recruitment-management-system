using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    public class AuthConfiguration : IEntityTypeConfiguration<Auth>
    {
        public void Configure(EntityTypeBuilder<Auth> builder)
        {
            builder.ToTable("Auth");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(a => a.UserName)
                .IsUnique();

            builder.OwnsOne(a => a.Email, email =>
            {
                email.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("Email");

                email.HasIndex(e => e.Address).IsUnique();
            });

            builder.Property(a => a.PasswordHash)
                .HasMaxLength(256);

            builder.Property(a => a.GoogleId)
                .HasMaxLength(100);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.UpdatedAt);

            builder.Property(a => a.LastLoginAt);

            builder.Property(a => a.DeletedAt);
        }
    }
}