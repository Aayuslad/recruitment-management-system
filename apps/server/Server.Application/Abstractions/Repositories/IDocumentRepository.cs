using Server.Domain.Entities.Documents;

namespace Server.Application.Abstractions.Repositories
{
    public interface IDocumentRepository
    {
        Task AddAsync(DocumentType documentType, CancellationToken cancellationToken);
        Task UpdateAsync(DocumentType documentType, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken);
        Task<DocumentType?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<DocumentType>> GetAllAsync(CancellationToken cancellationToken);
    }
}