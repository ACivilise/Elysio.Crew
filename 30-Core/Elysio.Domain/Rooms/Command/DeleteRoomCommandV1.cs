using Elysio.Core.Helpers;
using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Command;

public class DeleteRoomCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}

public class DeleteRoomCommandV1Validator
    : AbstractValidator<DeleteRoomCommandV1>
{
    public DeleteRoomCommandV1Validator()
    {
        RuleFor(a => a.UserEmail)
            .Matches(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")
            .WithMessage("Email invalid par rapport à la regex @\"^[\\w-]+(\\.[\\w-]+)*@([\\w-]+\\.)+[a-zA-Z]{2,7}$\"");
    }
}

public class DeleteRoomCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteRoomCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteRoomCommandV1, Unit>.Handle(
        DeleteRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var user = await CommandHelper.ValidateUser(dbContext, request.UserEmail);

        var room = await dbContext.Rooms.FirstOrDefaultAsync(a => a.CreatorId == user.Id && a.Id == request.Id);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        dbContext.Rooms.Remove(room);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}