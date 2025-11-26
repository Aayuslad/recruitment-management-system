using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Users;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRole");

            builder.HasKey(x => new { x.UserId, x.RoleId });

            builder.Property(ur => ur.AssignedAt)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany(x => x.Roles)
                .IsRequired()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Role)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.AssignedByUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ur => ur.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}