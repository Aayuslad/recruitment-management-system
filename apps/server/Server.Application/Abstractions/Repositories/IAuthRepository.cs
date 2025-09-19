using Server.Domain.Entities;

namespace Server.Application.Abstractions.Repositories
{
    public interface IAuthRepository
    {
        Task<Auth?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}