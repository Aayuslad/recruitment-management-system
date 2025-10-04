using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using Server.Application.Abstractions.Services;

namespace Server.Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Guid authId, string? userName)
        {
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim("authId", authId.ToString()),
            };

            if (!string.IsNullOrEmpty(userName))
            {
                claims.Append(new System.Security.Claims.Claim("userName", userName.ToString()));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}