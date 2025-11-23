using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Candidates;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class CandidateDocumentConfiguration : IEntityTypeConfiguration<CandidateDocument>
    {
        public void Configure(EntityTypeBuilder<CandidateDocument> builder)
        {
            builder.ToTable("CandidateDocument");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            builder.Property(x => x.Url)
                .IsRequired();

            builder.Property(x => x.IsVerified)
                .IsRequired();

            builder.HasOne(x => x.Candidate)
                .WithMany(x => x.Documents)
                .HasForeignKey(x => x.CandidateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.DocumentType)
                .WithMany()
                .HasForeignKey(x => x.DocumentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}