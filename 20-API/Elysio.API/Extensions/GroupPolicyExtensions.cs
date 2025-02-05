using Microsoft.AspNetCore.Authorization;

namespace Elysio.Extensions;

public static class GroupPolicyExtensions
{
    public static (AuthorizationPolicy, AuthorizationPolicy) AddGroupPolicyExtension(
        this IServiceCollection services,
        IConfiguration config)
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("User")
            .Build();

        var adminPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole("Admin")
            .Build();

        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("RequireGlobalAccessPolicy", policy);
            options.AddPolicy("AdminPolicy", adminPolicy);
        });
        return (policy, adminPolicy);
    }
}