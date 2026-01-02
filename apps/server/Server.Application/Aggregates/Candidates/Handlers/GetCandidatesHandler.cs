
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
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetCandidatesHandler(ICandidateRepository candidateRepository, IJobApplicationRepository jobApplicationRepository)
        {
            _candidateRepository = candidateRepository;
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result<List<CandidateSummaryDTO>>> Handle(GetCandidatesQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch all candidates
            var candidates = await _candidateRepository.GetAllAsync(cancellationToken);


            // step 2: map DTOs
            var candidateDtos = new List<CandidateSummaryDTO>();

            foreach (var candidate in candidates)
            {
                var candidateJobApplications = await _jobApplicationRepository.GetApplicationsByCandidateIdAsync(candidate.Id, cancellationToken);

                candidateDtos.Add(
                     new CandidateSummaryDTO
                     {
                         Id = candidate.Id,
                         Email = candidate.Email.ToString(),
                         FirstName = candidate.FirstName,
                         MiddleName = candidate.MiddleName,
                         LastName = candidate.LastName,
                         Gender = candidate.Gender,
                         ContactNumber = candidate.ContactNumber.ToString(),
                         Dob = candidate.Dob,
                         CollegeName = candidate.CollegeName,
                         ResumeUrl = candidate.ResumeUrl,
                         IsBgVerificationCompleted = candidate.IsBgVerificationCompleted,
                         CreatedAt = candidate.CreatedAt,
                         IsDocumentsVerified = candidate.Documents.All(x => x.IsVerified),
                         JobApplications = candidateJobApplications.Select(
                            selector: x => new JobApplicationSummaryForCandidateDTO
                            {
                                Status = x.Status
                            }
                         ).ToList()
                     }
                );
            }

            // step 3: return result
            return Result<List<CandidateSummaryDTO>>.Success(candidateDtos);
        }
    }
}