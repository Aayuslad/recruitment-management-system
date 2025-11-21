using MediatR;

using Server.Application.Users.Commands.DTOs;
using Server.Core.Results;

namespace Server.Application.Users.Commands
{
    public class LoginUserCommand : IRequest<Result<LoginUserDTO>>
    {
        public LoginUserCommand(string usernameOrEmail, string password)
        {
            UsernameOrEmail = usernameOrEmail;
            Password = password;
        }

        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}