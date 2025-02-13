using Elysio.API.RequestDelegates.Conversations;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class ConversationRoutes
{
    public const string _apiName = "api/conversations";

    internal static WebApplication MapConversationsEndpoints(this WebApplication webApplication, AuthorizationPolicy? policy = null)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetConversationsDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetConversationsDelegate.GetDelegate.Method);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetConversationDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetConversationDelegate.GetDelegate.Method);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateConversationDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(CreateConversationDelegate.GetDelegate.Method);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateConversationDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(UpdateConversationDelegate.GetDelegate.Method);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteConversationDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(DeleteConversationDelegate.GetDelegate.Method);

        return webApplication;
    }
}