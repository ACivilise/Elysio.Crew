using Elysio.API.RequestDelegates.Users;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class UsersRoutes
{
    public const string _apiName = "api/user";

    internal static WebApplication MapUsersEndpoints(this WebApplication webApplication, AuthorizationPolicy policy)
    {
        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetMeDelegate.GetDelegate).RequireAuthorization(policy);

        return webApplication;
    }
}