using MediatR;

using Server.Application.Aggregates.Events.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Events.Queries
{
    public class GetEventsQuery : IRequest<Result<List<EventSummaryDTO>>>
    {
    }
}