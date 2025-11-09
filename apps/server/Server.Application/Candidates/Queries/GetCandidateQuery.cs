using MediatR;

using Server.Application.Candidates.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Candidates.Queries
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