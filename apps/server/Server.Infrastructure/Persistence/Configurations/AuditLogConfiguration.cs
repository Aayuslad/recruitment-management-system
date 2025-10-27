using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLog");

            builder.HasKey(e => e.Id);

            builder.Property(x => x.EntityType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.EntityId)
                .IsRequired();

            builder.Property(x => x.Action)
                .HasConversion<string>()
                .IsRequired();

            // FK: AuditLog -> user (1:1)
            builder.HasOne<User>()
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.ChangedBy)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.ChangedAt)
                .IsRequired();
        }
    }
}