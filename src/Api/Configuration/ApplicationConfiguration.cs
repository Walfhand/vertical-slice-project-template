using System.Reflection;
using Engine.Logging;
using Engine.ProblemDetails;
using Engine.Wolverine;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration;

public static class ApplicationConfiguration
{
    public static WebApplicationBuilder AddEngineModules(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.Host.AddCustomLogging();
        builder.Services.AddCustomProblemDetails();
        builder.Host.UseCustomWolverine(builder.Configuration, assembly);

        return builder;
    }
}
