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
    public List<Guid> AgentIds { get; set; } = new();
}

public class CreateRoomCommandV1Validator : AbstractValidator<CreateRoomCommandV1>
{
    public CreateRoomCommandV1Validator()
    {
        RuleFor(a => a.Name).NotEmpty();
    }
}

public class CreateRoomCommandV1Handler(ApplicationDbContext dbContext) : IRequestHandler<CreateRoomCommandV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<CreateRoomCommandV1, RoomDTO>.Handle(
        CreateRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var agents = await dbContext.Agents
            .Where(a => request.AgentIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        var room = new Room
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow,
            Agents = agents
        };

        dbContext.Rooms.Add(room);
        await dbContext.SaveChangesAsync(cancellationToken);

        return room.ToDto();
    }
}