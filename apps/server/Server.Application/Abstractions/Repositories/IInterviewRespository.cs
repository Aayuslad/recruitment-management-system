using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IInterviewRespository
    {
        Task AddAsync(Interview interview, CancellationToken cancellationToken);
        Task UpdateAsync(Interview interview, CancellationToken cancellationToken);
        Task DeleteAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}