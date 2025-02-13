using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Elysio.Data;

public static class DbContextHelper
{
    /// <summary>
    /// Effectue les migrations automatiques des bases de données
    /// </summary>
    /// <param name="app">App builder</param>
    /// <returns><paramref name="app"/></returns>
    public static IServiceCollection MigrateDb(this IServiceCollection services)
    {
        using (var scope = services.BuildServiceProvider())
        {
            var applicationContext = scope.GetRequiredService<ApplicationDbContext>();
            //applicationContext.Database.EnsureCreated();
            var pendingMigrations = applicationContext.Database.GetPendingMigrations().ToList();
            if (pendingMigrations.Count > 0)
                applicationContext.Database.Migrate();
        }
        return services;
    }
}