using Elysio.Data;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Command;

public class DeleteRoomCommandV1 : IRequest<Unit>
{
    public Guid Id { get; set; }
}

public class DeleteRoomCommandV1Validator
    : AbstractValidator<DeleteRoomCommandV1>
{
    public DeleteRoomCommandV1Validator()
    {
    }
}

public class DeleteRoomCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<DeleteRoomCommandV1, Unit>
{
    async Task<Unit> IRequestHandler<DeleteRoomCommandV1, Unit>.Handle(
        DeleteRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms.FirstOrDefaultAsync(a => a.Id == request.Id);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        dbContext.Rooms.Remove(room);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}