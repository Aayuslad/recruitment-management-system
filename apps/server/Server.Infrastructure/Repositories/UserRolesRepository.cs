
using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories
{
    public class UserRolesRepository : IUserRolesRepository
    {
        Task<UserRole[]?> IUserRolesRepository.GetUserRolesByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}