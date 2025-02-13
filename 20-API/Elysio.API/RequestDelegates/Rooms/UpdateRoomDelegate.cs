using Elysio.Domain.Rooms.Command;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace Elysio.API.RequestDelegates.Rooms;

public class UpdateRoomDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<UpdateRoomDelegate>>();
        try
        {
            var command = await context.FromBody<UpdateRoomCommandV1>();

            var result = await mediator.Send(command, cancellationToken: context.RequestAborted);

            context.Response.StatusCode = (int)HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
    };
}