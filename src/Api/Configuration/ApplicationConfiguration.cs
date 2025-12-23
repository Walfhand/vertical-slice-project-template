using System.Reflection;
using Api.Configuration.Cqrs;
using Engine.Logging;
using Engine.ProblemDetails;
using Engine.Wolverine;
using QuickApi.Abstractions.Cqrs;
using QuickApi.Engine.Web;

namespace Api.Configuration;

internal static class ApplicationConfiguration
{
    public static WebApplicationBuilder AddEngineModules(this WebApplicationBuilder builder, Assembly assembly)
    {
        builder.Host.AddCustomLogging();
        builder.Services.AddCustomProblemDetails();
        builder.Services.AddMinimalEndpoints(options => { options.SetBaseApiPath("api/v1"); });
        builder.Services.AddHttpContextAccessor();
        builder.Host.UseCustomWolverine(builder.Configuration, assembly);

        builder.Services.AddScoped<IMessage, MessageService>();
        return builder;
    }

    public static WebApplication UseApplication(this WebApplication app)
    {
        app.UseMinimalEndpoints();
        return app;
    }
}