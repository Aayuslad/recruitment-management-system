
using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;

namespace Server.Infrastructure.Repositories
{
    internal class RolesRepository : IRolesRepository
    {
        Task<Role?> IRolesRepository.GetByIdAsync(Guid id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}