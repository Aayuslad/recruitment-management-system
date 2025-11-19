using MediatR;

using Server.Application.Events.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Events.Queries
{
    public class GetEventsQuery : IRequest<Result<List<EventSummaryDTO>>>
    {
    }
}