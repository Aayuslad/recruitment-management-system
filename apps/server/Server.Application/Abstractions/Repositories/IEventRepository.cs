using Server.Domain.Entities.Events;

namespace Server.Application.Abstractions.Repositories
{
    public interface IEventRepository
    {
        Task AddAsync(Event event_, CancellationToken cancellationToken);
        Task UpdateAsync(Event event_, CancellationToken cancellationToken);
        Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Event>> GetAllAsync(CancellationToken cancellationToken);
    }
}