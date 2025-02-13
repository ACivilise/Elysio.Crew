using Elysio.Domain.Agents.Command;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;

namespace Elysio.API.RequestDelegates.Agents;

public class DeleteAgentDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<DeleteAgentDelegate>>();
        try
        {
            var id = context.FromRoute<Guid>("id");
            var query = new DeleteAgentCommandV1
            {
                Id = id,
            };
            var result = await mediator.Send(query, cancellationToken: context.RequestAborted);

            if (result == null)
            {
                context.NotFound();
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