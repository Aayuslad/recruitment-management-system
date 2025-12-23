using Server.Domain.Entities.Interviews;

namespace Server.Application.Abstractions.Repositories
{
    public interface IInterviewRespository
    {
        Task AddAsync(Interview interview, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<Interview> interviews, CancellationToken cancellationToken);
        Task UpdateAsync(Interview interview, CancellationToken cancellationToken);
        Task DeleteAsync(Interview interview, CancellationToken cancellationToken);
        Task<Interview?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Interview>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<Interview>> GetAllByJobApplicationIdAsync(Guid jobApplicationId, CancellationToken cancellationToken);
    }
}