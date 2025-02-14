using Elysio.Entities;
using Elysio.Models.DTOs;
using Microsoft.VisualBasic;

namespace Elysio.Mappers;

public static class AgentMappers
{
    public static AgentDTO ToDto(this Agent agent, bool mapRoom = true)
    {
        if (agent is null)
            return default;

        return new AgentDTO
        {
            Id = agent.Id,
            CreatedAt = agent.CreatedAt,
            UpdatedAt = agent.UpdatedAt,
            Name = agent.Name,
            Prompt = agent.Prompt,
            Temperature = agent.Temperature,
            Model = agent.Model,
            Description = agent.Description,
            Rooms = mapRoom ? agent.Rooms?.Select(r => r.ToDto()).ToList() ?? [] : [],
            Messages = agent.Messages?.Select(m => m.ToDto()).ToList() ?? new List<MessageDTO>(),
        };
    }
}