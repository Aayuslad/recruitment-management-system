using MediatR;

using Server.Application.Aggregates.Candidates.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Candidates.Commands
{
    public class CreateCandidateCommand : IRequest<Result>
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; }
        public string ContactNumber { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string CollegeName { get; set; } = null!;
        public string ResumeUrl { get; set; } = null!;
        public ICollection<CandidateSkillDTO> Skills { get; set; } = new HashSet<CandidateSkillDTO>();
    }
}