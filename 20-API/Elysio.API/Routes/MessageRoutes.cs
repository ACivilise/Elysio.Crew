using Elysio.API.RequestDelegates.Messages;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class MessageRoutes
{
    public const string _apiName = "api/messages";

    internal static WebApplication MapMessagesEndpoints(this WebApplication webApplication, AuthorizationPolicy policy)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetMessagesDelegate.GetDelegate).RequireAuthorization(policy);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetMessageDelegate.GetDelegate).RequireAuthorization(policy);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateMessageDelegate.GetDelegate).RequireAuthorization(policy);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateMessageDelegate.GetDelegate).RequireAuthorization(policy);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteMessageDelegate.GetDelegate).RequireAuthorization(policy);

        return webApplication;
    }
}