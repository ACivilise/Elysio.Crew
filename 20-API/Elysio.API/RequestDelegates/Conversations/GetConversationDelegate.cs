using Azure;
using Elysio.Domain.Conversations.Query;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Elysio.API.RequestDelegates.Conversations;

public class GetConversationDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<GetConversationDelegate>>();
        try
        {
            var email = context.User.FindFirst(ClaimTypes.Upn)?.Value ?? context.User.FindFirst(ClaimTypes.Email)?.Value;
            var id = context.FromRoute<Guid>("id");
            var query = new GetConversationQueryV1
            {
                Id = id,
                UserEmail = email
            };
            var result = await mediator.Send(query, cancellationToken: context.RequestAborted);

            if (result == null)
            {
                context.NotFound();
                return;
            }

            await context.OK(result);
        }
        catch (ValidationException ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex, HttpStatusCode.PreconditionFailed);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
        catch (RequestFailedException ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex, (HttpStatusCode)ex.Status);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
    };
}