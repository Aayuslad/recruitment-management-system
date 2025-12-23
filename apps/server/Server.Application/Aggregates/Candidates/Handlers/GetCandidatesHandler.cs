
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Queries;
using Server.Application.Aggregates.Candidates.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class GetCandidatesHandler : IRequestHandler<GetCandidatesQuery, Result<List<CandidateSummaryDTO>>>
    {
        private readonly ICandidateRepository _candidateRepository;

        public GetCandidatesHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<Result<List<CandidateSummaryDTO>>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch all candidates
            var candidates = await _candidateRepository.GetAllAsync(cancellationToken);

            // step 2: map DTOs
            var candidateDtos = candidates.Select(
                    x => new CandidateSummaryDTO
                    {
                        Id = x.Id,
                        Email = x.Email.ToString(),
                        FirstName = x.FirstName,
                        MiddleName = x.MiddleName,
                        LastName = x.LastName,
                        Gender = x.Gender,
                        ContactNumber = x.ContactNumber.ToString(),
                        Dob = x.Dob,
                        CollegeName = x.CollegeName,
                        ResumeUrl = x.ResumeUrl,
                        IsBgVerificationCompleted = x.IsBgVerificationCompleted,
                        CreatedAt = x.CreatedAt
                    }
                ).ToList();

            // step 3: return result
            return Result<List<CandidateSummaryDTO>>.Success(candidateDtos);
        }
    }
}