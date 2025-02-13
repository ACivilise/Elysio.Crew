using Elysio.Data;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using Elysio.Models.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Command;

public class UpdateRoomCommandV1 : IRequest<RoomDTO>
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateRoomCommandV1Validator
    : AbstractValidator<UpdateRoomCommandV1>
{
    public UpdateRoomCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();
    }
}

public class UpdateRoomCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<UpdateRoomCommandV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<UpdateRoomCommandV1, RoomDTO>.Handle(
        UpdateRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms.FirstOrDefaultAsync(c => c.Id == request.Id);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        room.Name = request.Name;
        room.Description = request.Description;
        room.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return room.ToDto();
    }
}