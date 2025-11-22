using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _context;

        public DocumentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IDocumentRepository.AddAsync(DocumentType documentType, CancellationToken cancellationToken)
        {
            _context.DocumentTypes.Add(documentType);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IDocumentRepository.UpdateAsync(DocumentType documentType, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<bool> IDocumentRepository.ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            return _context.DocumentTypes.AnyAsync(x => x.Name == name, cancellationToken);
        }

        Task<List<DocumentType>> IDocumentRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.DocumentTypes
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        Task<DocumentType?> IDocumentRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.DocumentTypes
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}