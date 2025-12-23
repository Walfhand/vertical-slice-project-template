using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Engine.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Engine.ProblemDetails;

public static class ProblemDetailsConfig
{
    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, _) => false;
            options.Map<ValidationException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = ex.Message,
                Status = 400,
                Detail = string.Join(',', ex.Errors.Select(failure => failure.ErrorMessage)),
                Type = ex.GetType().Name
            });
            options.Map<CustomException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = $"A {ex.GetType()} error has occurred",
                Status = (int)ex.StatusCode,
                Detail = ex.Message,
                Type = ex.GetType().Name
            });
            options.MapToStatusCode<ArgumentNullException>(StatusCodes.Status400BadRequest);
            options.Map<Exception>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "An unexpected error has occurred",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message,
                Type = ex.GetType().Name
            });
        });
        return services;
    }
}