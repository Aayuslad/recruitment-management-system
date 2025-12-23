
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.JobOpenings.Queries;
using Server.Application.Aggregates.JobOpenings.Queries.DTOs;
using Server.Core.Results;

namespace Server.Application.Aggregates.JobOpenings.Handlers
{
    internal class GetJobOpeningsForRecruiterHandler : IRequestHandler<GetJobOpeningsForRecruiterQuery, Result<List<JobOpeningsDetailDTO>>>
    {
        private readonly IJobOpeningRepository _jobOpeningRepository;

        public GetJobOpeningsForRecruiterHandler(IJobOpeningRepository jobOpeningRepository)
        {
            _jobOpeningRepository = jobOpeningRepository;
        }

        public async Task<Result<List<JobOpeningsDetailDTO>>> Handle(GetJobOpeningsForRecruiterQuery request, CancellationToken cancellationToken)
        {
            // step 1: get job openings
            var jos = await _jobOpeningRepository.GetAllAsync(cancellationToken);

            // step 2: map dto
            var josDto = new List<JobOpeningsDetailDTO>();
            foreach (var jo in jos)
            {
                var joSummaryDto = new JobOpeningsDetailDTO
                {
                    Id = jo.Id,
                    Title = jo.Title,
                    Type = jo.Type,
                    DesignationId = jo.PositionBatch.DesignationId,
                    DesignationName = jo.PositionBatch.Designation.Name,
                    JobLocation = jo.PositionBatch.JobLocation,
                    CreatedById = jo.CreatedBy,
                    CreatedByUserName = jo.CreatedByUser?.Auth.UserName,
                    InterviewRounds = jo.InterviewRounds.Select(
                            selector: x => new InterviewRoundTemplateSummaryDetailDTO
                            {
                                RoundNumber = x.RoundNumber,
                                Type = x.Type,
                            }
                        ).ToList(),
                };
                josDto.Add(joSummaryDto);
            }

            // step 3: return result
            return Result<List<JobOpeningsDetailDTO>>.Success(josDto);
        }
    }
}