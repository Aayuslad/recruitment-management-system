using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class DocumentTypeConfiguration : AuditableEntityConfiguration<DocumentType>
    {
        public override void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            base.Configure(builder);

            builder.ToTable("DocumentType");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}