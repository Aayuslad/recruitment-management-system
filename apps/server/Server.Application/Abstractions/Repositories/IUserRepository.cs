using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        // auth (the auth table)
        Task AddAuthAsync(Auth auth, CancellationToken cancellationToken);
        Task<Auth?> GetAuthByAuthIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Auth?> GetAuthByEmailOrUserNameAsync(string value, CancellationToken cancellationToken);
        Task<bool> AuthExistsByEmailAsync(Email email, CancellationToken cancellationToken);
        Task<bool> AuthExistsByUserNameAsync(string userName, CancellationToken cancellationToken);

        // user profile (the user table)
        Task AddProfileAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetProfileByUserIdAsync(Guid id, CancellationToken cancellationToken);
        Task<User?> GetProfileByAuthIdAsync(Guid authId, CancellationToken cancellationToken);
        Task<bool> ProfileExistsByAuthIdAsync(Guid authId, CancellationToken cancellationToken);
        Task<bool> ProfileExistsByContactNumberAsync(ContactNumber contactNumber, CancellationToken cancellationToken);
    }
}