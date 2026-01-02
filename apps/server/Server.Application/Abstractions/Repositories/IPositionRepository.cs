using Server.Domain.Entities.Positions;

namespace Server.Application.Abstractions.Repositories
{
    public interface IPositionRepository
    {
        Task UpdateAsync(Position position, CancellationToken cancellationToken);
        Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Position>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<Position>> GetAllByBatchIdAsync(Guid batchId, CancellationToken cancellationToken);
    }
}