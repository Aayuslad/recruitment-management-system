
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Events.Queries;
using Server.Application.Events.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Events.Handlers
{
    internal class GetEventsHandler : IRequestHandler<GetEventsQuery, Result<List<EventSummaryDTO>>>
    {
        private readonly IEventRepository _repository;

        public GetEventsHandler(IEventRepository eventRepository)
        {
            _repository = eventRepository;
        }

        public async Task<Result<List<EventSummaryDTO>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch events
            var events = await _repository.GetAllAsync(cancellationToken);

            // step 2: map dtos
            var eventDtos = new List<EventSummaryDTO>();
            foreach (var e in events)
            {
                var eventDto = new EventSummaryDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                    JobOpenings = e.EventJobOpenings.Select(
                            selector: x => new EventJobOpeningDetailDTO
                            {
                                JobOpeningId = x.JobOpeningId,
                                JobOpeningTitle = x.JobOpening.Title,
                                DesignationId = x.JobOpening.PositionBatch.DesignationId,
                                DesignationName = x.JobOpening.PositionBatch.Designation.Name,
                            }
                        ).ToList(),
                };

                eventDtos.Add(eventDto);
            }

            // step 3: return result
            return Result<List<EventSummaryDTO>>.Success(eventDtos);
        }
    }
}