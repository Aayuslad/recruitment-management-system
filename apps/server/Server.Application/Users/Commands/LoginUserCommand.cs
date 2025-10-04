using MediatR;

using Server.Core.Results;

namespace Server.Application.Users.Commands
{
    public class LoginUserCommand : IRequest<Result<string>>
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