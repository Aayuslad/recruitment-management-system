


using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Interviews;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class InterviewRepository : IInterviewRepository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IInterviewRepository.AddAsync(Interview interview, CancellationToken cancellationToken)
        {
            _context.Interviews.Add(interview);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task AddRangeAsync(IEnumerable<Interview> interviews, CancellationToken cancellationToken)
        {
            _context.Interviews.AddRange(interviews);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IInterviewRepository.UpdateAsync(Interview interview, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IInterviewRepository.DeleteAsync(Interview interview, CancellationToken cancellationToken)
        {
            _context.Interviews.Remove(interview);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Interview?> IInterviewRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Interviews
                .AsTracking()
                .Include(x => x.JobApplication)
                   .ThenInclude(x => x.Candidate)
                .Include(x => x.JobApplication)
                   .ThenInclude(x => x.JobOpening)
                       .ThenInclude(x => x.PositionBatch)
                            .ThenInclude(x => x.Designation)
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.SkillFeedbacks)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.GivenByUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.Participants)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Auth)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<List<Interview>> GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Interviews
               .AsNoTracking()
               .Include(x => x.JobApplication)
                   .ThenInclude(x => x.Candidate)
               .Include(x => x.JobApplication)
                   .ThenInclude(x => x.JobOpening)
                       .ThenInclude(x => x.PositionBatch)
                            .ThenInclude(x => x.Designation)
               .Include(x => x.Participants)
               .ToListAsync(cancellationToken);
        }

        public Task<List<Interview>> GetAllByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken)
        {
            return _context.Interviews
                .AsTracking()
                .Include(x => x.JobApplication)
                    .ThenInclude(x => x.Candidate)
                .Include(x => x.JobApplication)
                    .ThenInclude(x => x.JobOpening)
                        .ThenInclude(x => x.PositionBatch)
                            .ThenInclude(x => x.Designation)
                .Include(x => x.Participants)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Auth)
                .Where(x => x.JobApplicationId == jobApplicationId)
                .ToListAsync(cancellationToken);
        }
    }
}