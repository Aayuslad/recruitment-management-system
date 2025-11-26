using Server.Domain.Entities.JobApplications;

namespace Server.Application.Abstractions.Repositories
{
    public interface IJobApplicationRepository
    {
        Task AddAsync(JobApplication jobApplication, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<JobApplication> applications, CancellationToken cancellationToken);
        Task UpdateAsync(JobApplication jobApplication, CancellationToken cancellationToken);
        Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> ExistsByCandidateAndOpeningAsync(Guid jobOpeningId, Guid candidateId, CancellationToken cancellationToken);
        Task<JobApplication?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<JobApplication>> GetAllAsync(CancellationToken cancellationToken);
    }
}