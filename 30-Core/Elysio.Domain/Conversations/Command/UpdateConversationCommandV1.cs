using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Command;

public class UpdateConversationCommandV1 : IRequest<ConversationDTO>
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }
}

public class UpdateConversationCommandV1Validator
    : AbstractValidator<UpdateConversationCommandV1>
{
    public UpdateConversationCommandV1Validator()
    {
    }
}

public class UpdateConversationCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<UpdateConversationCommandV1, ConversationDTO>
{
    async Task<ConversationDTO> IRequestHandler<UpdateConversationCommandV1, ConversationDTO>.Handle(
        UpdateConversationCommandV1 request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Conversations.FirstOrDefaultAsync(c => c.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Conversation", request.Id);

        agent.RoomId = request.RoomId;
        agent.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return agent.ToDto();
    }
}