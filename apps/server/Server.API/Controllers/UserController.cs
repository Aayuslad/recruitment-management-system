using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Application.Users.Commands;
using Server.Application.Users.Queries;
using Server.Core.Extensions;

namespace Server.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Value?.Token))
            {
                var token = result.Value.Token;

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Ok(new { message = "Registerd" });
            }

            return result.ToActionResult(this);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Value?.Token))
            {
                var token = result.Value.Token;

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Ok(new { message = "Logged In", IsPorofileCompleted = result.Value.IsProfileCompleted });
            }

            return result.ToActionResult(this);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });

            return Ok(new { message = "Logged out" });
        }

        [Authorize]
        [HttpPost("user-profile")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserProfileCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess && !string.IsNullOrEmpty(result.Value?.Token))
            {
                var token = result.Value.Token;

                Response.Cookies.Append("jwt", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(30)
                });

                return Ok(new { message = "profile created" });
            }

            return result.ToActionResult(this);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var authIdString = _httpContextAccessor.HttpContext?.User.FindFirst("authId")?.Value;

            if (string.IsNullOrEmpty(authIdString) || !Guid.TryParse(authIdString, out Guid authId))
                return Unauthorized(new { error = "Invalid token" });

            var query = new GetUserQuery(authId);
            var result = await _mediator.Send(query);

            return result.ToActionResult(this);
        }
    }
}