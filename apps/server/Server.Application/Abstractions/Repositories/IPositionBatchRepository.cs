using Server.Domain.Entities.Positions;

namespace Server.Application.Abstractions.Repositories
{
    public interface IPositionBatchRepository
    {
        Task AddAsync(PositionBatch positionBatch, CancellationToken cancellationToken);
        Task UpdateAsync(PositionBatch positionBatch, CancellationToken cancellationToken);
        Task<PositionBatch?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<PositionBatch>> GetAllAsync(CancellationToken cancellationToken);
    }
}