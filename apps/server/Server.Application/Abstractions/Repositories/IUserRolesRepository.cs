using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IUserRolesRepository
    {
        Task<UserRole[]?> GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}