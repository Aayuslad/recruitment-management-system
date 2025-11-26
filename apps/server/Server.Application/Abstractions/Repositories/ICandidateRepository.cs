using Server.Domain.Entities.Candidates;

namespace Server.Application.Abstractions.Repositories
{
    public interface ICandidateRepository
    {
        Task AddAsync(Candidate candidate, CancellationToken cancellationToken);
        Task UpdateAsync(Candidate candidate, CancellationToken cancellationToken);
        Task<Candidate?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Candidate>> GetAllAsync(CancellationToken cancellationToken);
    }
}