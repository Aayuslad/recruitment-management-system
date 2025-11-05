using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class DesignationConfiguration : AuditableEntityConfiguration<Designation>
    {
        public override void Configure(EntityTypeBuilder<Designation> builder)
        {
            base.Configure(builder);

            builder.ToTable("Designation");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.Description)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}