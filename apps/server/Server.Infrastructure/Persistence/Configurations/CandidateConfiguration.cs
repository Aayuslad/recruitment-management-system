using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class CandidateConfiguration : AuditableEntityConfiguration<Candidate>
    {
        public override void Configure(EntityTypeBuilder<Candidate> builder)
        {
            base.Configure(builder);

            builder.ToTable("Candidate");

            builder.HasKey(x => x.Id);
        }
    }
}