using Elysio.API.RequestDelegates.Conversations;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class ConversationRoutes
{
    public const string _apiName = "api/conversations";

    internal static WebApplication MapConversationsEndpoints(this WebApplication webApplication, AuthorizationPolicy policy)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetConversationsDelegate.GetDelegate).RequireAuthorization(policy);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetConversationDelegate.GetDelegate).RequireAuthorization(policy);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateConversationDelegate.GetDelegate).RequireAuthorization(policy);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateConversationDelegate.GetDelegate).RequireAuthorization(policy);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteConversationDelegate.GetDelegate).RequireAuthorization(policy);

        return webApplication;
    }
}