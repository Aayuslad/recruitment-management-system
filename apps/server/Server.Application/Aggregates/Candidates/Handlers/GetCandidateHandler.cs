
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Queries;
using Server.Application.Aggregates.Candidates.Queries.DTOs;
using Server.Application.Exeptions;
using Server.Core.Results;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class GetCandidateHandler : IRequestHandler<GetCandidateQuery, Result<CandidateDetailDTO>>
    {
        private readonly ICandidateRepository _candidateRepository;

        public GetCandidateHandler(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }

        public async Task<Result<CandidateDetailDTO>> Handle(GetCandidateQuery request, CancellationToken cancellationToken)
        {
            // step 1: fetch the candidate
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                throw new NotFoundExeption("Candidate Not Found");
            }

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
                        VerifiedBy = x.VerifiedBy
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
                ContactNumber = candidate.ContactNumber.ToString(),
                Dob = candidate.Dob,
                ResumeUrl = candidate.ResumeUrl,
                IsBgVerificationCompleted = candidate.IsBgVerificationCompleted,
                BgVerificationCompletedById = candidate.BgVerifiedById,
                BgVerificationCompletedByUserName = candidate.BgVerifiedByUser?.Auth.UserName,
                Skills = skillDtos,
                Documents = documentDtos
            };

            // step 3: return result
            return Result<CandidateDetailDTO>.Success(candidateDto);
        }
    }
}