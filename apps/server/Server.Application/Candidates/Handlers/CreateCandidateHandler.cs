
using MediatR;

using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Repositories;
using Server.Application.Candidates.Commands;
using Server.Core.Results;
using Server.Domain.Entities;
using Server.Domain.ValueObjects;

namespace Server.Application.Candidates.Handlers
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
                return Result.Failure("Unauthorised", 401);
            }

            // step 1: check VOs
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
                contactNumber: contactNumber,
                dob: request.Dob,
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