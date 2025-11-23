
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Interviews;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class InterviewRepository : IInterviewRespository
    {
        private readonly ApplicationDbContext _context;

        public InterviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IInterviewRespository.AddAsync(Interview interview, CancellationToken cancellationToken)
        {
            _context.Interviews.Add(interview);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IInterviewRespository.UpdateAsync(Interview interview, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IInterviewRespository.DeleteAsync(Interview interview, CancellationToken cancellationToken)
        {
            _context.Interviews.Remove(interview);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Interview?> IInterviewRespository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Interviews
                .AsTracking()
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.SkillFeedbacks)
                .Include(x => x.Feedbacks)
                    .ThenInclude(x => x.GivenByUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.Participants)
                    .ThenInclude(x => x.User)
                        .ThenInclude(x => x.Auth)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}