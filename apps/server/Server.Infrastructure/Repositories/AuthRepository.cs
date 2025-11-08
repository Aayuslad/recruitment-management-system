using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task<Auth?> IAuthRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Auths.FindAsync(new object?[] { id }, cancellationToken).AsTask();
        }

        Task<bool> IAuthRepository.ExistsByEmailAsync(Email email, CancellationToken cancellationToken)
        {
            return _context.Auths.AnyAsync(a => a.Email == email);
        }

        Task<bool> IAuthRepository.ExistsByUserNameAsync(string userName, CancellationToken cancellationToken)
        {
            return _context.Auths.AnyAsync(a => a.UserName == userName);
        }

        Task<Auth?> IAuthRepository.GetByUserNameOrEmail(string emailOrUserName, CancellationToken cancellationToken)
        {
            var emailVO = Email.Create(emailOrUserName).Value!;
            return _context.Auths
                .Where(a => a.UserName == emailOrUserName || a.Email == emailVO)
                .Select(a => a)
                .FirstOrDefaultAsync(cancellationToken);
        }

        Task IAuthRepository.AddAsync(Auth auth, CancellationToken cancellationToken)
        {
            _context.Auths.Add(auth);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}