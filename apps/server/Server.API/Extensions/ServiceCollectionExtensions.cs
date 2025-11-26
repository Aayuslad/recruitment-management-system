using System.Text;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Server.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config)
        {
            // authetication
            services.AddJwtAuth(config);

            // Controllers + JSON config
            services.AddControllers(opts =>
                {
                    // Prevents ASP.NET Core from automatically treating non-nullable reference types 
                    // (like string, object) as [Required]. Lets FluentValidation (or manual rules) 
                    // fully control "required" checks instead of framework forcing them.
                    opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // CORS
            services.AddCorsPolicies(config);

            // Swagger / Endpoints
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // HttpContext
            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication("JwtBearer")
                .AddJwtBearer("JwtBearer", options =>
                {
                    var key = config["Jwt:Key"];
                    if (string.IsNullOrWhiteSpace(key))
                        throw new InvalidOperationException("JWT Key is not configured");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };

                    // to read token from cookie instead of Authorization header
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (ctx.Request.Cookies.ContainsKey("jwt"))
                            {
                                ctx.Token = ctx.Request.Cookies["jwt"];
                            }
                            return Task.CompletedTask;
                        },

                        //TODO: Learn RBAC and implement properly
                        // OnTokenValidated = ctx =>
                        // {
                        //     ((ClaimsIdentity)ctx.Principal!.Identity!).AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                        //     return Task.CompletedTask;
                        // }
                    };
                });

            return services;
        }

        public static IServiceCollection AddCorsPolicies(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            return services;
        }
    }
}