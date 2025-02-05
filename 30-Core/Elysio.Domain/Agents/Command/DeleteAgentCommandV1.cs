using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Agents.Command;

public class DeleteAgentCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}

public class DeleteAgentCommandV1Validator
    : AbstractValidator<DeleteAgentCommandV1>
{
    public DeleteAgentCommandV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class DeleteAgentCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteAgentCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteAgentCommandV1, Unit>.Handle(
        DeleteAgentCommandV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);

        var agent = await dbContext.Agents.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (agent == null)
            throw new NotFoundException("Agent", request.Id);

        dbContext.Agents.Remove(agent);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}