using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(ur => ur.Id); // using Guid as PK for simplicity

            builder.Property(ur => ur.UserId).IsRequired();
            builder.Property(ur => ur.RoleId).IsRequired();
            builder.Property(ur => ur.AssignedBy).IsRequired();
            builder.Property(ur => ur.AssignedAt).IsRequired();

            // indexes
            builder.HasIndex(ur => ur.UserId);
            builder.HasIndex(ur => ur.RoleId);

            // relationships

            // User - Role ( n : m )
            builder.HasOne(x => x.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}