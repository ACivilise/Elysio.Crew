using Elysio.API.RequestDelegates.Rooms;
using Microsoft.AspNetCore.Authorization;

namespace Elysio.API.Routes;

public static class RoomRoutes
{
    public const string _apiName = "api/rooms";

    internal static WebApplication MapRoomsEndpoints(this WebApplication webApplication, AuthorizationPolicy policy)
    {
        // GetAll
        webApplication.MapGet($"/{_apiName}", GetRoomsDelegate.GetDelegate).RequireAuthorization(policy);

        // GetById
        webApplication.MapGet($"/{_apiName}/{{id}}", GetRoomDelegate.GetDelegate).RequireAuthorization(policy);

        // Create
        webApplication.MapPost($"/{_apiName}", CreateRoomDelegate.GetDelegate).RequireAuthorization(policy);

        // Update
        webApplication.MapPut($"/{_apiName}/{{id}}", UpdateRoomDelegate.GetDelegate).RequireAuthorization(policy);

        // Delete
        webApplication.MapDelete($"/{_apiName}/{{id}}", DeleteRoomDelegate.GetDelegate).RequireAuthorization(policy);

        return webApplication;
    }
}