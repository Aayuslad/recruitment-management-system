using MediatR;

using Server.Application.Aggregates.Designations.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Designations.Queries
{
    public class GetDesignationsQuery : IRequest<Result<IEnumerable<DesignationDetailDTO>>>
    {
        public GetDesignationsQuery(int? page, int? pageCount, string? search)
        {
            Page = page;
            PageCount = pageCount;
            Search = search;
        }

        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public string? Search { get; set; }
    }
}