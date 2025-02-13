using Elysio.API.RequestDelegates.Messages;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class MessageRoutes
{
    public const string _apiName = "api/messages";

    internal static WebApplication MapMessagesEndpoints(this WebApplication webApplication, AuthorizationPolicy? policy = null)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetMessagesDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetMessagesDelegate.GetDelegate.Method);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetMessageDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetMessageDelegate.GetDelegate.Method);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateMessageDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(CreateMessageDelegate.GetDelegate.Method);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateMessageDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(UpdateMessageDelegate.GetDelegate.Method);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteMessageDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(DeleteMessageDelegate.GetDelegate.Method);

        return webApplication;
    }
}