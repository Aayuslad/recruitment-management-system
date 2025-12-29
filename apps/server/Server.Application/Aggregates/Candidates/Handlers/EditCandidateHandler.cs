
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exceptions;
using Server.Core.Results;
using Server.Domain.Entities.Candidates;
using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class EditCandidateHandler : IRequestHandler<EditCandidateCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EditCandidateHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(EditCandidateCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedException();
            }

            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found");
            }

            // step 2: check VOs
            var contactNumber = ContactNumber.Create(request.ContactNumber);
            var email = Email.Create(request.Email);

            // step 2: prepare updated child collections

            // candidate skills
            var skills = request.Skills.Select(
                    x => CandidateSkill.Create(
                        candidateId: candidate.Id,
                        skillId: x.SkillId
                    )
                ).ToList();

            // candidate documents
            var documents = request.Documents.Select(
                    x => CandidateDocument.Create(
                        id: x.Id ?? Guid.NewGuid(),
                        candidateId: candidate.Id,
                        documentTypeId: x.DocumentTypeId,
                        url: x.Url
                    )
                ).ToList();

            // step 3: update aggregate root
            candidate.Update(
                updatedBy: Guid.Parse(userIdString),
                email: email,
                firstName: request.FirstName,
                middleName: request.MiddleName,
                lastName: request.LastName,
                gender: request.Gender,
                contactNumber: contactNumber,
                dob: request.Dob,
                collegeName: request.CollegeName,
                resumeUrl: request.ResumeUrl,
                skills: skills,
                documents: documents
            );

            // step 4: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 5: return result
            return Result.Success();
        }
    }
}