using Elysio.Domain.Rooms.Query;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elysio.API.RequestDelegates.Rooms;

public class GetRoomsDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var logger = serviceProvider.GetRequiredService<ILogger<GetRoomsDelegate>>();
        try
        {
            var includeAgents = context.Request.Query["agents"].ToString().ToLowerInvariant() == "true";
            var query = new GetRoomsQueryV1
            {
                IncludeAgents = includeAgents
            };
            var result = await mediator.Send(query, context.RequestAborted);

            if (result == null)
            {
                await context.NoContent(result);
                return;
            }

            await context.OK(result);
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting rooms: {Message}", ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
    };
}