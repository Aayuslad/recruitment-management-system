using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class AuthConfiguration : AuditableEntityConfiguration<Auth>
    {
        public override void Configure(EntityTypeBuilder<Auth> builder)
        {
            base.Configure(builder);

            builder.ToTable("Auth");

            builder.HasKey(a => a.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(a => a.UserName)
                .IsUnique();

            builder.Property(a => a.Email)
                .HasConversion(
                    emailVO => emailVO.ToString(),
                    email => Email.Create(email).Value!
                )
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnName("Email");
            builder.HasIndex(a => a.Email).IsUnique();

            builder.Property(a => a.PasswordHash)
                .HasMaxLength(256);

            builder.Property(a => a.GoogleId)
                .HasMaxLength(100);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            builder.Property(a => a.LastLoginAt);

            builder.Property(a => a.DeletedAt);
        }
    }
}