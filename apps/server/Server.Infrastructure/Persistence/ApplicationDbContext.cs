using Microsoft.EntityFrameworkCore;

using Server.Domain.Entities;

namespace Server.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // entities here
        public DbSet<Auth> Auths { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Designation> Designations { get; set; } = null!;
        public DbSet<DesignationSkill> DesignationSkills { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global soft-delete filter for all auditable entities
            // Soft delete filters
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Auth>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<Skill>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<Designation>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<DesignationSkill>().HasQueryFilter(ds => !ds.Skill.IsDeleted && !ds.Designation.IsDeleted);
            modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.User.IsDeleted);
            modelBuilder.Entity<AuditLog>().HasQueryFilter(al => !al.CreatedByUser.IsDeleted);


            base.OnModelCreating(modelBuilder);
        }
    }
}