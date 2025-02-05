﻿using Elysio.Entities;
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
            CreatorId = conversation.CreatorId,
            Creator = conversation.Creator?.ToDto(),
            UpdaterId = conversation.UpdaterId,
            Updater = conversation.Updater?.ToDto(),
            RoomId = conversation.RoomId,
            Name = conversation.Name,
            Room = conversation.Room?.ToDto(),
            Messages = conversation.Messages.Select(m => m.ToDto()).ToList(),
        };
    }
}