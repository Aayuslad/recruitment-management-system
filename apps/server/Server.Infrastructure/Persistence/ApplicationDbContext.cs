using Microsoft.EntityFrameworkCore;

using Server.Domain.Entities;
using Server.Domain.Entities.Candidates;
using Server.Domain.Entities.Designations;
using Server.Domain.Entities.Documents;
using Server.Domain.Entities.Employees;
using Server.Domain.Entities.Events;
using Server.Domain.Entities.Interviews;
using Server.Domain.Entities.JobApplications;
using Server.Domain.Entities.JobOpenings;
using Server.Domain.Entities.Notifications;
using Server.Domain.Entities.Positions;
using Server.Domain.Entities.Roles;
using Server.Domain.Entities.Skills;
using Server.Domain.Entities.Users;

namespace Server.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Auth> Auths { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Designation> Designations { get; set; } = null!;
        public DbSet<DesignationSkill> DesignationSkills { get; set; } = null!;
        public DbSet<PositionBatch> PositionBatches { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<PositionBatchReviewer> PositionBatchReviewers { get; set; } = null!;
        public DbSet<PositionStatusMoveHistory> PositionStatusMoveHistories { get; set; } = null!;
        public DbSet<SkillOverRide> SkillOverRides { get; set; } = null!;
        public DbSet<JobOpening> JobOpenings { get; set; } = null!;
        public DbSet<JobOpeningInterviewer> JobOpeningInterviewers { get; set; } = null!;
        public DbSet<InterviewPanelRequirement> InterviewPanelRequirements { get; set; } = null!;
        public DbSet<InterviewRoundTemplate> InterviewRoundTemplates { get; set; } = null!;
        public DbSet<Candidate> Candidates { get; set; } = null!;
        public DbSet<CandidateSkill> CandidateSkills { get; set; } = null!;
        public DbSet<CandidateDocument> CandidatesDocument { get; set; } = null!;
        public DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public DbSet<JobApplication> JobApplications { get; set; } = null!;
        public DbSet<JobApplicationStatusMoveHistory> JobApplicationStatusMoveHistories { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<SkillFeedback> SkillFeedbacks { get; set; } = null!;
        public DbSet<Interview> Interviews { get; set; } = null!;
        public DbSet<InterviewParticipant> InterviewParticipants { get; set; } = null!;
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventJobOpening> EventsJobOpening { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            // Global soft-delete filters for all auditable 

            // user Aggregate
            // TODO: update/fix the filters here, once you resolve User Aggregate 
            modelBuilder.Entity<User>()
                .HasQueryFilter(u => !u.IsDeleted);

            modelBuilder.Entity<Auth>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<UserRole>()
                .HasQueryFilter(ur => !ur.User.IsDeleted);

            // skill Aggregate
            modelBuilder.Entity<Skill>()
                .HasQueryFilter(s => !s.IsDeleted);

            // designation Aggregate
            modelBuilder.Entity<Designation>()
                .HasQueryFilter(d => !d.IsDeleted);

            modelBuilder.Entity<DesignationSkill>()
                .HasQueryFilter(x =>
                    !x.Designation.IsDeleted &&
                    !x.Skill.IsDeleted);

            // PositionBatch Aggregate
            modelBuilder.Entity<PositionBatch>()
                .HasQueryFilter(x =>
                    !x.IsDeleted &&
                    !x.Designation.IsDeleted);

            modelBuilder.Entity<PositionBatchReviewer>()
                .HasQueryFilter(x =>
                    !x.PositionBatch.IsDeleted);

            modelBuilder.Entity<Position>()
                .HasQueryFilter(x => !x.PositionBatch.IsDeleted);

            modelBuilder.Entity<PositionStatusMoveHistory>()
                .HasQueryFilter(x =>
                    !x.Position.PositionBatch.IsDeleted);

            modelBuilder.Entity<SkillOverRide>()
                .HasQueryFilter(x => !x.Skill.IsDeleted);

            // candidate Aggregate
            // TODO: add dependent entities for safety
            modelBuilder.Entity<Candidate>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<CandidateDocument>()
                .HasQueryFilter(x => !x.Candidate.IsDeleted);

            modelBuilder.Entity<CandidateSkill>()
                .HasQueryFilter(x => !x.Candidate.IsDeleted);

            // job opening aggregate
            // TODO: complete filters here...
            modelBuilder.Entity<JobOpening>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<JobOpeningInterviewer>()
                .HasQueryFilter(x => !x.JobOpening.IsDeleted);

            modelBuilder.Entity<InterviewRoundTemplate>()
                .HasQueryFilter(x => !x.JobOpening.IsDeleted);

            modelBuilder.Entity<InterviewPanelRequirement>()
                .HasQueryFilter(x => !x.InterviewRoundTemplate.JobOpening.IsDeleted);

            // job application
            modelBuilder.Entity<JobApplication>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<JobApplicationStatusMoveHistory>()
                .HasQueryFilter(x => !x.JobApplication.IsDeleted);

            modelBuilder.Entity<Interview>()
                .HasQueryFilter(x => !x.JobApplication.IsDeleted);

            // event
            modelBuilder.Entity<Event>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<EventJobOpening>()
                .HasQueryFilter(x =>
                    !x.Event.IsDeleted &&
                    !x.JobOpening.IsDeleted);

            // document types
            modelBuilder.Entity<DocumentType>()
                .HasQueryFilter(x => !x.IsDeleted);


            modelBuilder.Entity<Role>()
                .HasData(
                    Role.Create("Admin", "Manages users, roles, and system-wide configurations.", null, Guid.Parse("730a5c0e-76a2-4826-a192-3c584a367f82")),
                    Role.Create("Recruiter", "Manages job openings, candidate profiles, interviews.", null, Guid.Parse("d51ae472-657e-4b68-9c73-82be537ae083")),
                    Role.Create("Interviewer", "Provides interview feedback.", null, Guid.Parse("e8157fe7-7c8e-44ad-8440-891350dbc7e9 ")),
                    Role.Create("HR", "Culture fit, final negotiation, documentation and background verification", null, Guid.Parse("2508f54e-ab1f-481c-859f-4f1505533dfa")),
                    Role.Create("Reviewer", "Screens CVs and shortlists candidates.", null, Guid.Parse("761a2beb-ae68-4950-865b-98ea7eb7079a")),
                    Role.Create("Candidate", "Views job openings, uploads CVs, and submits documents.", null, Guid.Parse("78e3afbe-c2b2-459b-a114-1f23e4cd42f0 ")),
                    Role.Create("Viewer", "Read-only access to all data.", null, Guid.Parse("8fd55f0f-8262-4781-ba7b-7124db6f147b"))
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}