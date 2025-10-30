using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Core.Entities;
using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasOne<User>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.HasOne<User>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.LastUpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne<User>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);
        }
    }
}