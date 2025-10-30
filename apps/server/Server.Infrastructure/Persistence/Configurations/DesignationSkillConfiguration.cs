using Microsoft.EntityFrameworkCore;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence.Configurations
{
    public class DesignationSkillConfiguration : IEntityTypeConfiguration<DesignationSkill>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<DesignationSkill> builder)
        {
            builder.ToTable("DesignationSkill");

            builder.HasKey(ds => new { ds.DesignationId, ds.SkillId });

            builder.Property(ds => ds.SkillType)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(ds => ds.MinExperienceYears)
                .IsRequired(false);

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