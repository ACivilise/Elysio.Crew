using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Messages.Command;

public class DeleteMessageCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}

public class DeleteMessageCommandV1Validator
    : AbstractValidator<DeleteMessageCommandV1>
{
    public DeleteMessageCommandV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class DeleteMessageCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteMessageCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteMessageCommandV1, Unit>.Handle(
        DeleteMessageCommandV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);

        var message = await dbContext.Messages.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (message == null)
            throw new NotFoundException("Message", request.Id);

        dbContext.Messages.Remove(message);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}