using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Command;

public class DeleteAgentCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteAgentCommandV1Validator
    : AbstractValidator<DeleteAgentCommandV1>
{
    public DeleteAgentCommandV1Validator()
    {
    }
}

public class DeleteAgentCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteAgentCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteAgentCommandV1, Unit>.Handle(
        DeleteAgentCommandV1 request, CancellationToken cancellationToken)
    {
        var agent = await dbContext.Agents.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Agent", request.Id);

        dbContext.Agents.Remove(agent);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}