using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task<User?> IUserRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task<User?> IUserRepository.GetByAuthIdAsync(Guid authId, CancellationToken cancellationToken)
        {
            return _context.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.AuthId == authId, cancellationToken);
        }

        Task<bool> IUserRepository.ExistsByContactNumberAsync(ContactNumber contactNumber, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(u => u.ContactNumber == contactNumber, cancellationToken);
        }

        Task<bool> IUserRepository.ExistsByAuthId(Guid authId, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(u => u.AuthId == authId, cancellationToken);
        }

        Task IUserRepository.AddAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}