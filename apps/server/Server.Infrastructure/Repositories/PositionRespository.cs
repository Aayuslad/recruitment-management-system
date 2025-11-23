using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities.Positions;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class PositionRespository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;

        public PositionRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IPositionRepository.UpdateAsync(Position position, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Position?> IPositionRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Positions
                .Include(x => x.StatusMoveHistories)
                    .ThenInclude(x => x.MovedByUser.Auth)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.SkillOverRides)
                        .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                        .ThenInclude(x => x.DesignationSkills)
                            .ThenInclude(x => x.Skill)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Reviewers)
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
                    .ThenInclude(x => x.Reviewers)
                        .ThenInclude(x => x.ReviewerUser)
                            .ThenInclude(x => x.Auth)
                .Include(x => x.PositionBatch)
                    .ThenInclude(x => x.Designation)
                .Include(x => x.StatusMoveHistories)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}