using Elysio.Services;
using Elysio.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Elsio.Crew.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services)
    {
        services.AddScoped<IAgentsService, AgentsService>();
        services.AddScoped<IOllamaService, OllamaService>();

        return services;
    }
}