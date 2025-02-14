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
    public string Name { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
}

public class UpdateConversationCommandV1Validator : AbstractValidator<UpdateConversationCommandV1>
{
    public UpdateConversationCommandV1Validator()
    {
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Id).NotEmpty();
        RuleFor(a => a.RoomId).NotEmpty();
    }
}

public class UpdateConversationCommandV1Handler(ApplicationDbContext dbContext) : IRequestHandler<UpdateConversationCommandV1, ConversationDTO>
{
    async Task<ConversationDTO> IRequestHandler<UpdateConversationCommandV1, ConversationDTO>.Handle(
        UpdateConversationCommandV1 request, CancellationToken cancellationToken)
    {
        var conversation = await dbContext.Conversations
            .Include(c => c.Room)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (conversation == null)
            throw new NotFoundException("Conversation", request.Id);

        // Verify the room exists
        var room = await dbContext.Rooms.FindAsync(new object[] { request.RoomId }, cancellationToken);
        if (room == null)
            throw new NotFoundException("Room", request.RoomId);

        conversation.Name = request.Name;
        conversation.RoomId = request.RoomId;
        conversation.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return conversation.ToDto();
    }
}