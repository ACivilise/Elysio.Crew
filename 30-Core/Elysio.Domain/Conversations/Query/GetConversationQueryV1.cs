using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Query;

public class GetConversationQueryV1 : IRequest<ConversationDTO>
{
    public Guid Id { get; set; }
}

public class GetConversationQueryV1Validator
    : AbstractValidator<GetConversationQueryV1>
{
    public GetConversationQueryV1Validator()
    {
    }
}

public class GetConversationQueryV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<GetConversationQueryV1, ConversationDTO>
{
    async Task<ConversationDTO> IRequestHandler<GetConversationQueryV1, ConversationDTO>.Handle(
        GetConversationQueryV1 request, CancellationToken cancellationToken)
    {
        var conversation = await dbContext.Conversations.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (conversation == null)
            throw new NotFoundException("Conversation", request.Id);

        return conversation.ToDto();
    }
}