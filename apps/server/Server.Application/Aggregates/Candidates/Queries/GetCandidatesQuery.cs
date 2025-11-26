using MediatR;

using Server.Application.Aggregates.Candidates.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Queries
{
    public class GetCandidatesQuery : IRequest<Result<List<CandidateSummaryDTO>>>
    {
        // add pagination fealds
    }
}