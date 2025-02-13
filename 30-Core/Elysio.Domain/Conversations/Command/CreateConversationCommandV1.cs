using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Command;

public class CreateConversationCommandV1 : IRequest<ConversationDTO>
{
    public string Name { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
}

public class CreateConversationCommandV1Validator
    : AbstractValidator<CreateConversationCommandV1>
{
    public CreateConversationCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();
    }
}

public class CreateConversationCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<CreateConversationCommandV1, ConversationDTO>
{
    async Task<ConversationDTO> IRequestHandler<CreateConversationCommandV1, ConversationDTO>.Handle(
        CreateConversationCommandV1 request, CancellationToken cancellationToken)
    {
        // on crée une conversation associé à cet utilisateur
        var newId = Guid.NewGuid();
        dbContext.Conversations.Add(new Conversation
        {
            Id = newId,
            Name = request.Name,
            RoomId = request.RoomId,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var newConversation = await dbContext.Conversations.FirstAsync(c => c.Id == newId);

        return newConversation.ToDto();
    }
}