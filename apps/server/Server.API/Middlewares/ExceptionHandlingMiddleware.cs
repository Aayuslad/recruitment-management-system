using Server.Application.Exeptions;
using Server.Domain.Exeptions;

namespace Server.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = ex.Errors.First().ErrorMessage });
            }
            catch (UnAuthorisedExeption ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ForbiddenExeption ex)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (NotFoundExeption ex)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (BadRequestExeption ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (ConflictExeption ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (DomainExeption ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Unhandled exception occurred while processing request: {Path} {ex}",
                    context.Request.Path, ex
                );

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error" });
            }
        }
    }
}