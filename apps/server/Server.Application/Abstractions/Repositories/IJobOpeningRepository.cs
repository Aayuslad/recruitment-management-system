using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IJobOpeningRepository
    {
        Task AddAsync(JobOpening jobOpening, CancellationToken cancellationToken);
        Task UpdateAysnc(JobOpening jobOpening, CancellationToken cancellationToken);
        Task<JobOpening?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<JobOpening>> GetAllAsync(CancellationToken cancellationToken);
    }
}