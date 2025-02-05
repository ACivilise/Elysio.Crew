using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Enums;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Command;

public class CreateMessageCommandV1 : IRequest<MessageDTO>
{
    public string UserEmail { get; set; } = string.Empty;
    public RolesEnum Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public Guid? AgentId { get; set; }
    public Guid ConversationId { get; set; }
}

public class CreateMessageCommandV1Validator
    : AbstractValidator<CreateMessageCommandV1>
{
    public CreateMessageCommandV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class CreateMessageCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<CreateMessageCommandV1, MessageDTO>
{
    async Task<MessageDTO> IRequestHandler<CreateMessageCommandV1, MessageDTO>.Handle(
        CreateMessageCommandV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);

        // on crée une conversation associé à cet utilisateur
        var newId = Guid.NewGuid();
        dbContext.Messages.Add(new Message
        {
            Id = newId,
            Content = request.Content,
            Role = request.Role,
            AgentId = request.AgentId,
            ConversationId = request.ConversationId,
            CreatorId = user.Id,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var newMessage = await dbContext.Messages.FirstAsync(c => c.Id == newId);

        return newMessage.ToDto();
    }
}