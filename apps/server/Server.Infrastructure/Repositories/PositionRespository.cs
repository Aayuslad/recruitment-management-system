using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    public class PositionRespository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IPositionRepository.UpdateAsync(Position position, CancellationToken cancellationToken)
        {
            // TEMP FIX
            _context.Set<PositionStatusMoveHistory>().AddRange(position.PositionStatusMoveHistories);
            // TODO: solve bug, somehow EF not traking the changes properly. it is updating insted of adding column in Hstory
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Position?> IPositionRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Positions
                .Include(x => x.PositionStatusMoveHistories)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.SkillOverRides)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                        .ThenInclude(x => x.DesignationSkills)
                            .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.PositionBatchReviewers)
                        .ThenInclude(x => x.ReviewerUser)
                            .ThenInclude(x => x.Auth)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<List<Position>> IPositionRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Positions
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.SkillOverRides)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.PositionBatchReviewers)
                        .ThenInclude(x => x.ReviewerUser)
                            .ThenInclude(x => x.Auth)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                .Include(x => x.PositionStatusMoveHistories)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}