
using MediatR;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Core.Results;
using Server.Domain.Entities.Candidates;
using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class EditCandidateHandler : IRequestHandler<EditCandidateCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUserContext _userContext;

        public EditCandidateHandler(ICandidateRepository candidateRepository, IUserContext userContext)
        {
            _candidateRepository = candidateRepository;
            _userContext = userContext;
        }

        public async Task<Result> Handle(EditCandidateCommand request, CancellationToken cancellationToken)
        {
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
                documents: documents,
                updatedBy: _userContext.UserId
            );

            // step 4: persist entity
            await _candidateRepository.UpdateAsync(candidate, cancellationToken);

            // step 5: return result
            return Result.Success();
        }
    }
}