
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class CandidateRepository : ICandidateRepository
    {
        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task ICandidateRepository.AddAsync(Candidate candidate, CancellationToken cancellationToken)
        {
            _context.Candidates.Add(candidate);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task ICandidateRepository.UpdateAsync(Candidate candidate, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Candidate?> ICandidateRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Candidates
                .AsTracking()
                .Include(x => x.BgVerifiedByUser!)
                    .ThenInclude(x => x.Auth)
                .Include(x => x.Skills)
                    .ThenInclude(x => x.Skill)
                .Include(x => x.Documents)
                    .ThenInclude(x => x.DocumentType)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<Candidate>> ICandidateRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Candidates
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        }
    }
}