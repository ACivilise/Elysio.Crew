using Elysio.API.RequestDelegates.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace Elysio.API.Routes;

public static class RoomRoutes
{
    public const string _apiName = "api/rooms";

    internal static WebApplication MapRoomsEndpoints(this WebApplication webApplication, AuthorizationPolicy? policy = null)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetRoomsDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithOpenApi(operation => new(operation)
            {
                Parameters = new List<OpenApiParameter>
                {
                    new()
                    {
                        Name = "agents",
                        In = ParameterLocation.Query,
                        Required = false,
                        Schema = new() { Type = "boolean" }
                    }
                }
            })
            .WithMetadata(GetRoomsDelegate.GetDelegate.Method);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetRoomDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(GetRoomDelegate.GetDelegate.Method);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateRoomDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(CreateRoomDelegate.GetDelegate.Method);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateRoomDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(UpdateRoomDelegate.GetDelegate.Method);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteRoomDelegate.GetDelegate)
            // .RequireAuthorization(policy)
            .WithMetadata(DeleteRoomDelegate.GetDelegate.Method);

        return webApplication;
    }
}