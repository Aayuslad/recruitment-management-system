using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    public class SkillConfiguration : AuditableEntityConfiguration<Skill>
    {
        public override void Configure(EntityTypeBuilder<Skill> builder)
        {
            base.Configure(builder);

            builder.ToTable("Skill");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.Description).IsRequired();
        }
    }
}
