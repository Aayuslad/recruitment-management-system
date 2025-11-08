using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task AddAsync(User user, CancellationToken cancellationToken = default);

        Task<bool> ExistsByContactNumberAsync(ContactNumber contactNumber, CancellationToken cancellationToken = default);
        Task<bool> ExistsByAuthId(Guid authId, CancellationToken cancellationToken = default);
        Task<User?> GetByAuthIdAsync(Guid authId, CancellationToken cancellationToken = default);
    }
}