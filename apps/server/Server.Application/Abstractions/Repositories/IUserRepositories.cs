using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IUserRepositories
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellation = default);
    }
}