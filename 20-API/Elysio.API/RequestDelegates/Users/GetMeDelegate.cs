using Azure;
using Elysio.Domain.Users.Query;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Elysio.API.RequestDelegates.Users;

public class GetMeDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<GetMeDelegate>>();

        try
        {
            var email = context.User.FindFirst(ClaimTypes.Upn)?.Value ?? context.User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
                throw new Exception("No UPN in the claims");
            var name = context.User.FindFirst(ClaimTypes.GivenName)?.Value ?? "Name";
            var lastName = context.User.FindFirst(ClaimTypes.Surname)?.Value ?? "LastName";
            // On génère la commande
            var command = new GetMeQueryV1
            {
                UserEmail = email,
                Name = name,
                LastName = lastName,
            };
            // On l'envoie au médiateur et on récupère la réponse
            var me = await mediator.Send(command, cancellationToken: context.RequestAborted);

            if (me == null)
            {
                context.NotFound();
                return;
            }

            await context.OK(me);
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