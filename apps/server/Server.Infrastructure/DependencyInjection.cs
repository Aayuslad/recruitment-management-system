using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Server.Application.Abstractions.Repositories;
using Server.Application.Abstractions.Services;
using Server.Infrastructure.Persistence;
using Server.Infrastructure.Repositories;
using Server.Infrastructure.Security;

namespace Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            // Register Repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IDesignationRepository, DesignationRepository>();
            services.AddScoped<IPositionBatchRepository, PositionBatchRepository>();
            services.AddScoped<IPositionRepository, PositionRespository>();
            services.AddScoped<IJobOpeningRepository, JobOpeningRepository>();
            services.AddScoped<ICandidateRepository, CandidateRepository>();
            services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
            services.AddScoped<IInterviewRespository, InterviewRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            // Register Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IHasher, Hasher>();

            return services;
        }
    }
}