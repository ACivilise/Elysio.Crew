
using Elysio.Services.Interfaces;
using FluentValidation;
using MediatR;

namespace Elysio.Domain.Conversations.Command;

public class StartConversationStreamCommandV1 : IStreamRequest<string>
{
    public Guid ConversationId { get; set; }
    public string InitialMessage { get; set; } = string.Empty;
}

public class StartConversationStreamCommandV1Validator : AbstractValidator<StartConversationStreamCommandV1>
{
    public StartConversationStreamCommandV1Validator()
    {
        RuleFor(x => x.ConversationId).NotEmpty();
        RuleFor(x => x.InitialMessage).NotEmpty();
    }
}

public class StartConversationStreamCommandV1Handler(IAgentsService agentsService) : IStreamRequestHandler<StartConversationStreamCommandV1, string>
{
    public IAsyncEnumerable<string> Handle(StartConversationStreamCommandV1 request, CancellationToken cancellationToken)
    {
        return agentsService.ChatWithAgents(request.InitialMessage, request.ConversationId);
    }
}