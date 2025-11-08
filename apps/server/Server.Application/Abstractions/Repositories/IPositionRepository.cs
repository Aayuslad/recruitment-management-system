using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IPositionRepository
    {
        Task UpdateAsync(Position position, CancellationToken cancellationToken);
        Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Position>> GetAllAsync(CancellationToken cancellationToken);
    }
}