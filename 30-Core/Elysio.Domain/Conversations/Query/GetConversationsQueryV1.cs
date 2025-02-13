using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Query;

public class GetConversationsQueryV1 : IRequest<IReadOnlyList<ConversationDTO>>
{
}

public class GetConversationsQueryV1Validator
    : AbstractValidator<GetConversationsQueryV1>
{
    public GetConversationsQueryV1Validator()
    {
    }
}

public class GetConversationsQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetConversationsQueryV1, IReadOnlyList<ConversationDTO>>
{
    async Task<IReadOnlyList<ConversationDTO>> IRequestHandler<GetConversationsQueryV1, IReadOnlyList<ConversationDTO>>.Handle(
        GetConversationsQueryV1 request, CancellationToken cancellationToken)
    {
        var conversations = dbContext.Conversations;
        return await conversations.Select(a => a.ToDto()).ToListAsync();
    }
}