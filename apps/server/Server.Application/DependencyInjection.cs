using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Server.Application.Common.Behaviors;
using Server.Application.Designations.Commands;
using Server.Application.Designations.Queries;
using Server.Application.Designations.Validators;
using Server.Application.Skills.Commands;
using Server.Application.Skills.Queries;
using Server.Application.Skills.Validators;
using Server.Application.Users.Commands;
using Server.Application.Users.Validators;

namespace Server.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            // validators
            services.AddTransient<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
            services.AddTransient<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
            services.AddTransient<IValidator<CreateUserProfileCommand>, CreateUserProfileCommandValidator>();
            services.AddTransient<IValidator<GetSkillQuery>, GetSkillQueryValidator>();
            services.AddTransient<IValidator<GetSkillsQuery>, GetSkillsQueryValidator>();
            services.AddTransient<IValidator<CreateSkillCommand>, CreateSkillCommandValidator>();
            services.AddTransient<IValidator<EditSkillCommand>, EditSkillCommandValidator>();
            services.AddTransient<IValidator<DeleteSkillCommand>, DeleteSkillCommandValidator>();
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