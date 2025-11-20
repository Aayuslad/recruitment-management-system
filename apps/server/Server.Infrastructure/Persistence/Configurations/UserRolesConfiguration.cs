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

            builder.HasKey(ur => ur.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(ur => ur.AssignedAt)
                .IsRequired();

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

            builder.HasIndex(ur => ur.UserId);
            builder.HasIndex(ur => ur.RoleId);

            builder.HasOne(x => x.AssignedByUser)
                .WithMany()
                .IsRequired()
                .HasForeignKey(ur => ur.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}