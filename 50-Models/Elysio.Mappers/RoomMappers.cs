using Elysio.Entities;
using Elysio.Models.DTOs;

namespace Elysio.Mappers;

public static class RoomMappers
{
    public static RoomDTO ToDto(this Room room)
    {
        if (room is null)
            return default;

        return new RoomDTO
        {
            Id = room.Id,
            CreatedAt = room.CreatedAt,
            UpdatedAt = room.UpdatedAt,
            CreatorId = room.CreatorId,
            Creator = room.Creator?.ToDto(),
            UpdaterId = room.UpdaterId,
            Updater = room.Updater?.ToDto(),
            Name = room.Name,
            Description = room.Description,
            Agents = room.Agents.Select(a => a.ToDto()).ToList(),
            Conversations = room.Conversations.Select(a => a.ToDto()).ToList()
        };
    }
}