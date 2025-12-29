
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Queries;
using Server.Application.Aggregates.Candidates.Queries.DTOs;
using Server.Application.Exceptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class GetCandidateHandler : IRequestHandler<GetCandidateQuery, Result<CandidateDetailDTO>>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetCandidateHandler(ICandidateRepository candidateRepository, IJobApplicationRepository jobApplicationRepository)
        {
            _candidateRepository = candidateRepository;
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<Result<CandidateDetailDTO>> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the candidate
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundException("Candidate Not Found");
            }

            var candidateJobApplications = await _jobApplicationRepository.GetApplicationsByCandidateIdAsync(candidate.Id, cancellationToken);

            // step 2: map dto

            // skills
            var skillDtos = candidate.Skills.Select(
                    x => new CandidateSkillDetailDTO
                    {
                        SkillId = x.SkillId,
                        SkillName = x.Skill.Name,
                    }
                ).ToList();

            // documents
            var documentDtos = candidate.Documents.Select(
                    x => new CandidateDocumentDetailDTO
                    {
                        Id = x.Id,
                        Url = x.Url,
                        DocumentTypeId = x.DocumentTypeId,
                        DocumentName = x.DocumentType.Name,
                        IsVerified = x.IsVerified,
                        VerifiedById = x.VerifiedById,
                        VerifiedByUserName = x.VerifiedByUser?.Auth.UserName
                    }
                ).ToList();

            // job applications
            var jobApplicationDtos = candidateJobApplications.Select(
                    x => new JobApplicationDetailForCandidateDTO
                    {
                        Id = x.Id,
                        DesignationName = x.JobOpening.PositionBatch.Designation.Name,
                        AppliedAt = x.AppliedAt,
                        JobLocation = x.JobOpening.PositionBatch.JobLocation,
                        Status = x.Status,
                    }
            ).ToList();

            // aggregate root
            var candidateDto = new CandidateDetailDTO
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
                BgVerificationCompletedById = candidate.BgVerifiedById,
                BgVerificationCompletedByUserName = candidate.BgVerifiedByUser?.Auth.UserName,
                CreatedAt = candidate.CreatedAt,
                Skills = skillDtos,
                Documents = documentDtos,
                JobApplications = jobApplicationDtos
            };

            // step 3: return result
            return Result<CandidateDetailDTO>.Success(candidateDto);
        }
    }
}