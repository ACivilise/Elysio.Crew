using Elysio.API.RequestDelegates.Agents;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class AgentRoutes
{
    public const string _apiName = "api/agents";

    internal static WebApplication MapAgentsEndpoints(this WebApplication webApplication, AuthorizationPolicy? policy = null)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetAgentsDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetAgentsDelegate.GetDelegate.Method);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetAgentDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetAgentDelegate.GetDelegate.Method);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateAgentDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(CreateAgentDelegate.GetDelegate.Method);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateAgentDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(UpdateAgentDelegate.GetDelegate.Method);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteAgentDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(DeleteAgentDelegate.GetDelegate.Method);

        return webApplication;
    }
}