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
        public DbSet<Designation> Designations { get; set; } = null!;
        public DbSet<DesignationSkill> DesignationSkills { get; set; } = null!;
        public DbSet<PositionBatch> PositionBatchs { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<PositionBatchReviewers> PositionReviewers { get; set; } = null!;
        public DbSet<PositionStatusMoveHistory> PositionStatusMoveHistories { get; set; } = null!;
        public DbSet<SkillOverRide> SkillOverRides { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global soft-delete filters for all auditable 

            // user Aggregate 
            // #TODO: update/fix the filters here, once you resolve User Aggregate 
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Auth>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<UserRole>().HasQueryFilter(ur => !ur.User.IsDeleted);

            // skill Aggregate
            modelBuilder.Entity<Skill>().HasQueryFilter(s => !s.IsDeleted);

            // designation Aggregate
            modelBuilder.Entity<Designation>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<DesignationSkill>().HasQueryFilter(x => !x.Designation.IsDeleted);

            modelBuilder.Entity<DesignationSkill>().HasQueryFilter(x => !x.Skill.IsDeleted);

            // PositionBatch Aggregate
            modelBuilder.Entity<PositionBatch>().HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<PositionBatchReviewers>().HasQueryFilter(x => !x.PositionBatch.IsDeleted);
            modelBuilder.Entity<Position>().HasQueryFilter(x => !x.PositionBatch.IsDeleted);
            modelBuilder.Entity<SkillOverRide>().HasQueryFilter(x => !x.PositionBatch.IsDeleted);
            modelBuilder.Entity<PositionStatusMoveHistory>().HasQueryFilter(x => !x.Position.PositionBatch.IsDeleted);

            modelBuilder.Entity<SkillOverRide>().HasQueryFilter(x => !x.Skill.IsDeleted);
            modelBuilder.Entity<PositionBatch>().HasQueryFilter(x => !x.Designation.IsDeleted);

            // candidate Aggregate
            modelBuilder.Entity<Candidate>().HasQueryFilter(d => !d.IsDeleted);


            base.OnModelCreating(modelBuilder);
        }
    }
}