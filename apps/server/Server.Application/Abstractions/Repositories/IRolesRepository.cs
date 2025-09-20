using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IRolesRepository
    {
        Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellation = default);
    }
}