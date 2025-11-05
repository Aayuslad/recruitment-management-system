
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.JobOpenings.Queries;
using Server.Application.JobOpenings.Queries.DTOs.ForCandiateClient;
using Server.Core.Results;

namespace Server.Application.JobOpenings.Handlers
{
    internal class GetJobOpeningsForCandidateHandler : IRequestHandler<GetJobOpeningsForCandidateQuery, Result<List<JobOpeningsDetailDTO>>>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public GetJobOpeningsForCandidateHandler(IJobOpeningRepository jobOpeningRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
        }

        public async Task<Result<List<JobOpeningsDetailDTO>>> Handle(GetJobOpeningsForCandidateQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the job openings
            var jos = await _jobOpeningRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var josDto = new List<JobOpeningsDetailDTO>();

            foreach (var jo in jos)
            {
                var joDto = new JobOpeningsDetailDTO
                {
                    Id = jo.Id,
                    Title = jo.Title,
                    DesignationName = jo.PositionBatch.Designation.Name,
                    Type = jo.Type,
                };
                josDto.Add(joDto);
            }

            // step 3: return result
            return Result<List<JobOpeningsDetailDTO>>.Success(josDto);
        }
    }
}