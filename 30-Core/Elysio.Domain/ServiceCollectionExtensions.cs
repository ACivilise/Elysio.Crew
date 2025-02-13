using Microsoft.Extensions.DependencyInjection;

namespace Elsio.Crew.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}