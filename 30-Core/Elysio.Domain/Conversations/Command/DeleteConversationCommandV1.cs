using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Conversations.Command;

public class DeleteConversationCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteConversationCommandV1Validator
    : AbstractValidator<DeleteConversationCommandV1>
{
    public DeleteConversationCommandV1Validator()
    {
    }
}

public class DeleteConversationCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteConversationCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteConversationCommandV1, Unit>.Handle(
        DeleteConversationCommandV1 request, CancellationToken cancellationToken)
    {
        var conversation = await dbContext.Conversations.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (conversation == null)
            throw new NotFoundException("Conversation", request.Id);

        dbContext.Conversations.Remove(conversation);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}