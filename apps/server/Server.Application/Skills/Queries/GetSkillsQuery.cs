using MediatR;

using Server.Application.DTOs;
using Server.Core.Results;

namespace Server.Application.Skills.Queries
{
    public class GetSkillsQuery : IRequest<Result<IEnumerable<SkillDTO>>>
    {
        public GetSkillsQuery(int? page, int? pageCount, string? search) 
        {
            Page = page;
            PageCount = pageCount;
            Search = search;
        }

        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public string? Search {  get; set; }
    }
}
