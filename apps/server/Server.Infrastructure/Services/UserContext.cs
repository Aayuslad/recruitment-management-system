using Microsoft.AspNetCore.Http;

using Server.Application.Abstractions.Services;
using Server.Application.Exceptions;

namespace Server.Infrastructure.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;
                if (string.IsNullOrWhiteSpace(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                {
                    throw new UnAuthorisedException();
                }
                return userId;
            }
        }

        public Guid AuthId
        {
            get
            {
                var authIdString = _httpContextAccessor.HttpContext?.User.FindFirst("authId")?.Value;
                if (string.IsNullOrWhiteSpace(authIdString) || !Guid.TryParse(authIdString, out Guid authId))
                {
                    throw new UnAuthorisedException();
                }
                return authId;
            }
        }

        public string? UserName
        {
            get
            {
                var userName = _httpContextAccessor.HttpContext?.User.FindFirst("userName")?.Value;
                return userName;
            }
        }
    }
}