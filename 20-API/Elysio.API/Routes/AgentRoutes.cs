using Elysio.API.RequestDelegates.Agents;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class AgentRoutes
{
    public const string _apiName = "api/agents";

    internal static WebApplication MapAgentsEndpoints(this WebApplication webApplication, AuthorizationPolicy policy)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetAgentsDelegate.GetDelegate).RequireAuthorization(policy);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetAgentDelegate.GetDelegate).RequireAuthorization(policy);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateAgentDelegate.GetDelegate).RequireAuthorization(policy);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateAgentDelegate.GetDelegate).RequireAuthorization(policy);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteAgentDelegate.GetDelegate).RequireAuthorization(policy);

        return webApplication;
    }
}