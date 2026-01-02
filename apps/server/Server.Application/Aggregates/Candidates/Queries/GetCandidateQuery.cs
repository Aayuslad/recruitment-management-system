using MediatR;

using Server.Application.Aggregates.Candidates.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Queries
{
    public class GetCandidateQuery : IRequest<Result<CandidateDetailDTO>>
    {
        public GetCandidateQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}