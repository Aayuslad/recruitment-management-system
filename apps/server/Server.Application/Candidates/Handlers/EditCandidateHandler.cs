
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Candidates.Handlers
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
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: fetch the entity
            var candidate = await _candidateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (candidate == null)
            {
                return Result.Failure("Candidate not found", 404);
            }

            // step 2: check VOs
            var result = ContactNumber.Create(request.ContactNumber);
            if (result.IsSuccess == false)
            {
                return Result.Failure(result.ErrorMessage ?? "Invalid contact number", result.StatusCode);
            }
            var contactNumber = result.Value!;

            var emailResult = Email.Create(request.Email);
            if (emailResult.IsSuccess == false)
            {
                return Result.Failure(emailResult.ErrorMessage ?? "Invalid Email", emailResult.StatusCode);
            }
            var email = emailResult.Value!;

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
                contactNumber: contactNumber,
                dob: request.Dob,
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