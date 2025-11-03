using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    public class PositionBatchRepository : IPositionBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionBatchRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IPositionBatchRepository.AddAsync(PositionBatch positionBatch, CancellationToken cancellationToken)
        {
            _context.PositionBatchs.Add(positionBatch);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IPositionBatchRepository.UpdateAsync(PositionBatch positionBatch, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<PositionBatch?> IPositionBatchRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.PositionBatchs
                .AsTracking()
                .Include(x => x.Designation)
                    .ThenInclude(x => x.DesignationSkills)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatchReviewers)
                    .ThenInclude(x => x.ReviewerUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.SkillOverRides)
                    .ThenInclude(x => x.Skill)
                .Include(x => x.Positions)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<PositionBatch>> IPositionBatchRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.PositionBatchs
                .AsNoTracking()
                .Include(x => x.Designation)
                    .ThenInclude(x => x.DesignationSkills)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatchReviewers)
                    .ThenInclude(x => x.ReviewerUser)
                        .ThenInclude(x => x.Auth)
                .Include(x => x.SkillOverRides)
                    .ThenInclude(x => x.Skill)
                .Include(x => x.Positions)
                .ToListAsync(cancellationToken);
        }
    }
}