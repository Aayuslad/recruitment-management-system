using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Server.Application.Aggregates.Designations.Commands;
using Server.Application.Aggregates.Designations.Queries;
using Server.Application.Aggregates.Designations.Validators;
using Server.Application.Aggregates.Skills.Commands;
using Server.Application.Aggregates.Skills.Queries;
using Server.Application.Aggregates.Skills.Validators;
using Server.Application.Aggregates.Users.Commands;
using Server.Application.Aggregates.Users.Commands.DTOs;
using Server.Application.Aggregates.Users.Validators;
using Server.Application.Common.Behaviors;

namespace Server.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            // validators

            // users 
            services.AddTransient<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
            services.AddTransient<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
            services.AddTransient<IValidator<CreateUserProfileCommand>, CreateUserProfileCommandValidator>();
            services.AddTransient<IValidator<UserRolesDTO>, RolesDTOValidator>();

            // skills
            services.AddTransient<IValidator<GetSkillsQuery>, GetSkillsQueryValidator>();
            services.AddTransient<IValidator<CreateSkillCommand>, CreateSkillCommandValidator>();
            services.AddTransient<IValidator<EditSkillCommand>, EditSkillCommandValidator>();
            services.AddTransient<IValidator<DeleteSkillCommand>, DeleteSkillCommandValidator>();

            // designations
            services.AddTransient<IValidator<GetDesignationQuery>, GetDesignationQueryValidator>();
            services.AddTransient<IValidator<GetDesignationsQuery>, GetDesignationsQueryValidator>();
            services.AddTransient<IValidator<CreateDesignationCommand>, CreateDesignationCommandValidator>();
            services.AddTransient<IValidator<EditDesignationCommand>, EditDesignationCommandValidator>();
            services.AddTransient<IValidator<DeleteDesignationCommand>, DeleteDesignationCommandValidator>();

            // pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}