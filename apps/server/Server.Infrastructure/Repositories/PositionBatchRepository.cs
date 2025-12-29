using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Positions;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class PositionBatchRepository : IPositionBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionBatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IPositionBatchRepository.AddAsync(PositionBatch positionBatch, CancellationToken cancellationToken)
        {
            _context.PositionBatches.Add(positionBatch);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IPositionBatchRepository.UpdateAsync(PositionBatch positionBatch, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<PositionBatch?> IPositionBatchRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.PositionBatches
                .AsTracking()
                .Include(x => x.Designation)
                    .ThenInclude(x => x.DesignationSkills)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.Reviewers)
                    .ThenInclude(x => x.ReviewerUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.SkillOverRides)
                    .ThenInclude(x => x.Skill)
                .Include(x => x.Positions)
                .Include(x => x.CreatedByUser!)
                    .ThenInclude(x => x.Auth)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<PositionBatch>> IPositionBatchRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.PositionBatches
                .AsNoTracking()
                .Include(x => x.Designation)
                .Include(x => x.Positions)
                .Include(x => x.CreatedByUser!)
                    .ThenInclude(x => x.Auth)
                .ToListAsync(cancellationToken);
        }
    }
}