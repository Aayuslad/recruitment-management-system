using MediatR;

using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Core.Results;
using Server.Domain.Enums;

namespace Server.Application.Aggregates.Users.Commands
{
    public class CreateUserProfileCommand : IRequest<Result<CreateUserProfileDTO>>
    {
        public CreateUserProfileCommand(
            string firstName,
            string? middleName,
            string lastName,
            string contactNumber,
            Gender? gender,
            DateTime dob
        )
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            ContactNumber = contactNumber;
            Gender = gender;
            Dob = dob;
        }

        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNumber { get; set; }
        public Gender? Gender { get; set; }
        public DateTime Dob { get; set; }
    }
}