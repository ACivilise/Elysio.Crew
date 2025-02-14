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
    public List<Guid> AgentIds { get; set; } = new();
}

public class UpdateRoomCommandV1Validator : AbstractValidator<UpdateRoomCommandV1>
{
    public UpdateRoomCommandV1Validator()
    {
        RuleFor(a => a.Name).NotEmpty();
        RuleFor(a => a.Id).NotEmpty();
    }
}

public class UpdateRoomCommandV1Handler(ApplicationDbContext dbContext) : IRequestHandler<UpdateRoomCommandV1, RoomDTO>
{
    async Task<RoomDTO> IRequestHandler<UpdateRoomCommandV1, RoomDTO>.Handle(
        UpdateRoomCommandV1 request, CancellationToken cancellationToken)
    {
        var room = await dbContext.Rooms
            .Include(r => r.Agents)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (room == null)
            throw new NotFoundException("Room", request.Id);

        room.Name = request.Name;
        room.Description = request.Description;
        room.UpdatedAt = DateTimeOffset.UtcNow;

        // Update agent relationships
        var agents = await dbContext.Agents
            .Where(a => request.AgentIds.Contains(a.Id))
            .ToListAsync(cancellationToken);

        room.Agents.Clear();
        foreach (var agent in agents)
        {
            room.Agents.Add(agent);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return room.ToDto();
    }
}