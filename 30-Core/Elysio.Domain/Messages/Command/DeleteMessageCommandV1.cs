using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Command;

public class DeleteMessageCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteMessageCommandV1Validator
    : AbstractValidator<DeleteMessageCommandV1>
{
    public DeleteMessageCommandV1Validator()
    {
    }
}

public class DeleteMessageCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteMessageCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteMessageCommandV1, Unit>.Handle(
        DeleteMessageCommandV1 request, CancellationToken cancellationToken)
    {
        var message = await dbContext.Messages.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (message == null)
            throw new NotFoundException("Message", request.Id);

        dbContext.Messages.Remove(message);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}