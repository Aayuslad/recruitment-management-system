
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Aggregates.Candidates.Commands;
using Server.Application.Exeptions;
using Server.Core.Results;
using Server.Domain.Entities.Candidates;
using Server.Domain.ValueObjects;

namespace Server.Application.Aggregates.Candidates.Handlers
{
    internal class CreateCandidateHandler : IRequestHandler<CreateCandidateCommand, Result>
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateCandidateHandler(ICandidateRepository candidateRepository, IHttpContextAccessor contextAccessor)
        {
            _candidateRepository = candidateRepository;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<Result> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
            if (userIdString == null)
            {
                throw new UnAuthorisedExeption();
            }

            // step 1: check VOs
            var contactNumber = ContactNumber.Create(request.ContactNumber);
            var email = Email.Create(request.Email);

            // step 2: create entity
            var newCandidateId = Guid.NewGuid();

            // create candiidate skills entity
            var skills = request.Skills.Select(
                    selector: x => CandidateSkill.Create(
                            candidateId: newCandidateId,
                            skillId: x.SkillId
                        )
                ).ToList();

            // create root entity
            var candidate = Candidate.Create(
                id: newCandidateId,
                createdBy: Guid.Parse(userIdString),
                email: email,
                firstName: request.FirstName,
                middleName: request.MiddleName,
                lastName: request.LastName,
                gender: request.Gender,
                contactNumber: contactNumber,
                dob: request.Dob,
                collegeName: request.CollegeName,
                resumeUrl: request.ResumeUrl,
                skills: skills
            );

            // step 3: persist entity
            await _candidateRepository.AddAsync(candidate, cancellationToken);

            // step 4: return result
            return Result.Success();
        }
    }
}