using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasOne(x => x.CreatedByUser)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.UpdatedByUser)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.LastUpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(x => x.DeletedByUser)
                .WithMany()
                .IsRequired(false)
                .HasForeignKey(s => s.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.DeletedAt)
                .IsRequired(false);
        }
    }
}