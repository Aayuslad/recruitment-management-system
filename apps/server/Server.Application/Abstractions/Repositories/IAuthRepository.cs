using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Abstractions.Repositories
{
    public interface IAuthRepository
    {
        Task<Auth?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> ExistsByUserNameAsync(string userName, CancellationToken cancellationToken = default);
        Task<Auth?> GetByUserNameOrEmail(string emailOrUserName, CancellationToken cancellationToken = default);
        Task AddAsync(Auth auth, CancellationToken cancellationToken = default);
    }
}