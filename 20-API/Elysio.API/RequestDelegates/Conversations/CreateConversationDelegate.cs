using Azure;
using Elysio.Domain.Conversations.Command;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Elysio.API.RequestDelegates.Conversations;

public class CreateConversationDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<CreateConversationDelegate>>();
        try
        {
            var email = context.User.FindFirst(ClaimTypes.Upn)?.Value ?? context.User.FindFirst(ClaimTypes.Email)?.Value;
            var command = await context.FromBody<CreateConversationCommandV1>();
            command.UserEmail = email;
            var result = await mediator.Send(command, cancellationToken: context.RequestAborted);

            context.Response.StatusCode = (int)HttpStatusCode.Created;
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