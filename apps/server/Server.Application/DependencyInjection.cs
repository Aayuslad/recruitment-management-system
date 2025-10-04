using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Server.Application.Common.Behaviors;
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
            services.AddTransient<IValidator<CreateUserCommand>, CreateUserCommandValidator>();

            //// pipeline
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}