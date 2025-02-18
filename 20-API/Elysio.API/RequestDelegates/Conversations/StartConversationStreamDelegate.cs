using Elysio.Domain.Conversations.Command;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;
using System.Text.Json;

namespace Elysio.API.RequestDelegates.Conversations;

public class StartConversationStreamDelegate
{
    public static RequestDelegate GetDelegate => async context =>
    {
        var serviceProvider = context.RequestServices;
        var mediator = serviceProvider.GetService<IMediator>();
        var logger = serviceProvider.GetService<ILogger<StartConversationStreamDelegate>>();
        try
        {
            var command = await context.FromBody<StartConversationStreamCommandV1>();

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "text/event-stream";
            context.Response.Headers.Add("Connection", "keep-alive");
            context.Response.Headers.Add("Cache-Control", "no-cache");

            var responseStream = mediator.CreateStream(command, context.RequestAborted);

            try
            {
                await foreach (var message in responseStream.WithCancellation(context.RequestAborted))
                {
                    if (context.RequestAborted.IsCancellationRequested)
                        break;

                    var messageData = message.Split('|');
                    var data = new { content = messageData[0], role = messageData[1], agentName = messageData[2] };
                    var serializedData = JsonSerializer.Serialize(data);
                    var sseData = $"data: {serializedData}\n\n";
                    
                    await context.Response.WriteAsync(sseData, context.RequestAborted);
                    await context.Response.Body.FlushAsync(context.RequestAborted);
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Client disconnected from conversation stream");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            logger.LogTrace(ex.StackTrace);
            await context.KO(ex);
        }
    };
}