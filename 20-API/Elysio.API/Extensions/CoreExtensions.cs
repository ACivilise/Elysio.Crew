using Elysio.Services;
using Elysio.Services.Interfaces;

namespace Elysio.API.Extensions;

public static class CoreExtensions
{
    internal static void ExtractConnectionStringInfo(this IServiceCollection services)
    {
        services.AddScoped<IAgentsService, AgentsService>();
    }
}