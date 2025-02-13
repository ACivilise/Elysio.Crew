using Elysio.Domain.Messages.Query;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elysio.API.RequestDelegates.Messages;

public class GetMessagesDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<GetMessagesDelegate>>();
        try
        {
            var query = new GetMessagesQueryV1
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