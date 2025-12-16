
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.JobOpenings;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class JobOpeningRepository : IJobOpeningRepository
    {
        private readonly ApplicationDbContext _context;

        public JobOpeningRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IJobOpeningRepository.AddAsync(JobOpening jobOpening, CancellationToken cancellationToken)
        {
            _context.JobOpenings.Add(jobOpening);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IJobOpeningRepository.UpdateAysnc(JobOpening jobOpening, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<JobOpening?> IJobOpeningRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.JobOpenings
                .AsTracking()
                .Include(x => x.JobOpeningInterviewers)
                    .ThenInclude(x => x.InterviewerUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                        .ThenInclude(x => x.DesignationSkills)
                            .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Positions)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.SkillOverRides)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.InterviewRounds)
                    .ThenInclude(x => x.PanelRequirements)
                .Include(x => x.SkillOverRides)
                    .ThenInclude(x => x.Skill)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<JobOpening>> IJobOpeningRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.JobOpenings
                .AsNoTracking()
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                .Include(x => x.InterviewRounds)
                .Include(x => x.CreatedByUser!)
                    .ThenInclude(x => x.Auth)
                .ToListAsync(cancellationToken);
        }
    }
}