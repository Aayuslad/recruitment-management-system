using MediatR;

using Server.Application.Candidates.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.ValueObjects;

namespace Server.Application.Candidates.Commands
{
    public class CreateCandidateCommand : IRequest<Result>
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string ResumeUrl { get; set; } = null!;
        public ICollection<CandidateSkillDTO> Skills { get; set; } = new HashSet<CandidateSkillDTO>();
    }
}