
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class JobApplicationRepository : IJobApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IJobApplicationRepository.AddAsync(JobApplication jobApplication, CancellationToken cancellationToken)
        {
            _context.JobApplications.Add(jobApplication);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IJobApplicationRepository.AddRangeAsync(IEnumerable<JobApplication> applications, CancellationToken cancellationToken)
        {
            _context.JobApplications.AddRange(applications);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IJobApplicationRepository.UpdateAsync(JobApplication jobApplication, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<bool> IJobApplicationRepository.ExistsByCandidateAndOpeningAsync(Guid jobOpeningId, Guid candidateId, CancellationToken cancellationToken)
        {
            return _context.JobApplications.AnyAsync(
                    x => x.JobOpeningId == jobOpeningId && x.CandidateId == candidateId,
                    cancellationToken
                );
        }

        Task<bool> IJobApplicationRepository.ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.JobApplications.AnyAsync(x => x.Id == id, cancellationToken);
        }

        Task<JobApplication?> IJobApplicationRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.JobApplications
                .AsTracking()
                .Include(x => x.Candidate)
                .Include(x => x.JobOpening)
                    .ThenInclude(x => x.PositionBatch)
                        .ThenInclude(x => x.Designation)
                .Include(x => x.StatusMoveHistories)
                    .ThenInclude(x => x.MovedByUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.SkillFeedbacks)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.GivenByUser)
                        .ThenInclude(x => x.Auth)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<JobApplication>> IJobApplicationRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.JobApplications
                .AsNoTracking()
                .Include(x => x.Candidate)
                .Include(x => x.JobOpening)
                    .ThenInclude(x => x.PositionBatch)
                        .ThenInclude(x => x.Designation)
                .ToListAsync(cancellationToken);
        }
    }
}