using MediatR;

using Server.Application.Candidates.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Candidates.Queries
{
    public class GetCandidatesQuery : IRequest<Result<List<CandidateSummaryDTO>>>
    {
        // add pagination fealds
    }
}