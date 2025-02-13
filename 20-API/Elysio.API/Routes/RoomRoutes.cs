using Elysio.API.RequestDelegates.Rooms;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class RoomRoutes
{
    public const string _apiName = "api/rooms";

    internal static WebApplication MapRoomsEndpoints(this WebApplication webApplication, AuthorizationPolicy? policy = null)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetRoomsDelegate.GetDelegate)
            // .RequireAuthorization(policy)
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