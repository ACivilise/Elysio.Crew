using Elysio.Data;
using Elysio.Entities;
using Elysio.Mappers;
using Elysio.Models.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elysio.Domain.Rooms.Command;

public class CreateRoomCommandV1 : IRequest<RoomDTO>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateRoomCommandV1Validator
    : AbstractValidator<CreateRoomCommandV1>
{
    public CreateRoomCommandV1Validator()
    {
        RuleFor(a => a.Name)
            .NotNull();
    }
}

public class CreateRoomCommandV1Handler(ApplicationDbContext dbContext)
    : IRequestHandler<CreateRoomCommandV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<CreateRoomCommandV1, RoomDTO>.Handle(
        CreateRoomCommandV1 request, CancellationToken cancellationToken)
    {
        // on crée une conversation associé à cet utilisateur
        var newId = Guid.NewGuid();
        dbContext.Rooms.Add(new Room
        {
            Id = newId,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        var newRoom = await dbContext.Rooms.FirstAsync(c => c.Id == newId);

        return newRoom.ToDto();
    }
}