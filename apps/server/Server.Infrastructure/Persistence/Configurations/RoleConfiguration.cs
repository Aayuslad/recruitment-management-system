using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Roles;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class RoleConfiguration : AuditableEntityConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.ToTable("Role");

            builder.HasKey(r => r.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(r => r.Name)
                .IsUnique();

            builder.Property(r => r.Description)
                .IsRequired(false)
                .HasMaxLength(200);
        }
    }
}