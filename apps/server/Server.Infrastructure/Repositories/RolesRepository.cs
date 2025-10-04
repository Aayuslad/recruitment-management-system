
using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        Task<Role?> IRolesRepository.GetByIdAsync(Guid id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}