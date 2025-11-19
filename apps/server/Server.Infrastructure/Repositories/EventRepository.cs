
using Microsoft.EntityFrameworkCore;

using Server.Application.Abstractions.Repositories;
using Server.Domain.Entities;
using Server.Infrastructure.Persistence;

namespace Server.Infrastructure.Repositories
{
    internal class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        Task IEventRepository.AddAsync(Event event_, CancellationToken cancellationToken)
        {
            _context.Events.Add(event_);
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task IEventRepository.UpdateAsync(Event event_, CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        Task<Event?> IEventRepository.GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Events
                .AsTracking()
                .Include(x => x.EventJobOpenings)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        Task<List<Event>> IEventRepository.GetAllAsync(CancellationToken cancellationToken)
        {
            return _context.Events
                .AsNoTracking()
                .Include(x => x.EventJobOpenings)
                    .ThenInclude(x => x.JobOpening)
                        .ThenInclude(x => x.PositionBatch)
                            .ThenInclude(x => x.Designation)
                .ToListAsync(cancellationToken);
        }
    }
}