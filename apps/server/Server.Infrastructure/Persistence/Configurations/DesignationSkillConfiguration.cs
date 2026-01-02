using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Server.Domain.Entities.Designations;

namespace Server.Infrastructure.Persistence.Configurations
{
    internal class DesignationSkillConfiguration : IEntityTypeConfiguration<DesignationSkill>
    {
        public void Configure(EntityTypeBuilder<DesignationSkill> builder)
        {
            builder.ToTable("DesignationSkill");

            builder.HasKey(ds => new { ds.DesignationId, ds.SkillId });

            builder.Property(ds => ds.SkillType)
                .HasConversion<string>()
                .IsRequired();

            builder.HasOne(x => x.Designation)
                .WithMany(d => d.DesignationSkills)
                .HasForeignKey(ds => ds.DesignationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Skill)
                .WithMany()
                .HasForeignKey(ds => ds.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}