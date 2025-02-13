using Elysio.Entities;
using Elysio.Models.DTOs;

namespace Elysio.Mappers;

public static class ConversationMappers
{
    public static ConversationDTO ToDto(this Conversation conversation)
    {
        if (conversation is null)
            return default;

        return new ConversationDTO
        {
            Id = conversation.Id,
            CreatedAt = conversation.CreatedAt,
            UpdatedAt = conversation.UpdatedAt,
            RoomId = conversation.RoomId,
            Name = conversation.Name,
            Room = conversation.Room?.ToDto(),
            Messages = conversation.Messages.Select(m => m.ToDto()).ToList(),
        };
    }
}