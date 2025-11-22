
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // user auth (auth table)
        Task IUserRepository.AddAuthAsync(Auth auth, CancellationToken cancellationToken)
        {
            _context.Auths.Add(auth);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<bool> IUserRepository.AuthExistsByEmailAsync(Email email, CancellationToken cancellationToken)
        {
            return _context.Auths.AnyAsync(x => x.Email == email, cancellationToken);
        }

        Task<bool> IUserRepository.AuthExistsByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return _context.Auths.AnyAsync(x => x.UserName == userName, cancellationToken);
        }

        Task<Auth?> IUserRepository.GetAuthByAuthIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Auths.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<Auth?> IUserRepository.GetAuthByEmailOrUserNameAsync(string emailOrUserName, CancellationToken cancellationToken)
        {
            var emailVO = Email.Create(emailOrUserName).Value!;
            return _context.Auths
                .FirstOrDefaultAsync(a => a.UserName == emailOrUserName || a.Email == emailVO, cancellationToken);
        }

        // user profile (user table)
        Task IUserRepository.AddProfileAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Add(user);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateProfileAsync(User user, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
        
        Task<User?> IUserRepository.GetProfileByAuthIdAsync(Guid authId, CancellationToken cancellationToken)
        {
            return _context.Users
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.AuthId == authId, cancellationToken);
        }

        Task<User?> IUserRepository.GetProfileByUserIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Users
                .AsTracking()
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        Task<bool> IUserRepository.ProfileExistsByAuthIdAsync(Guid authId, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(x => x.AuthId == authId, cancellationToken);
        }

        Task<bool> IUserRepository.ProfileExistsByContactNumberAsync(ContactNumber contactNumber, CancellationToken cancellationToken)
        {
            return _context.Users.AnyAsync(x => x.ContactNumber == contactNumber, cancellationToken);
        }
    }
}