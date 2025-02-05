using Elysio.Entities;
using Elysio.Models.DTOs;

namespace Elysio.Mappers;

public static class MessageMappers
{
    public static MessageDTO ToDto(this Message message)
    {
        if (message is null)
            return default;

        return new MessageDTO
        {
            Id = message.Id,
            CreatedAt = message.CreatedAt,
            UpdatedAt = message.UpdatedAt,
            CreatorId = message.CreatorId,
            Creator = message.Creator?.ToDto(),
            UpdaterId = message.UpdaterId,
            Updater = message.Updater?.ToDto(),
            Role = message.Role,
            Content = message.Content,
            AgentId = message.AgentId,
            Agent = message.Agent?.ToDto(),
            ConversationId = message.ConversationId,
            Conversation = message.Conversation?.ToDto(),
        };
    }
}