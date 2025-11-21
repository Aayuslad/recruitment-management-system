using MediatR;

using Server.Application.Users.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Users.Commands
{
    public sealed class RegisterUserCommand : IRequest<Result<RegisterUserDTO>>
    {
        public RegisterUserCommand(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}