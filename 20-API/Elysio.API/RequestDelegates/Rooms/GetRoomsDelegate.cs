using Elysio.Domain.Rooms.Query;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elysio.API.RequestDelegates.Rooms;

public class GetRoomsDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<GetRoomsDelegate>>();
        try
        {
            var query = new GetRoomsQueryV1
            {
            };
            var result = await mediator.Send(query, cancellationToken: context.RequestAborted);

            if (result == null)
            {
                await context.NoContent(result);
                return;
            }

            await context.OK(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
    };
}