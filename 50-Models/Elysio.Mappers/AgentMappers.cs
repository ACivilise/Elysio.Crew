using Elysio.Entities;
using Elysio.Models.DTOs;

namespace Elysio.Mappers;

public static class AgentMappers
{
    public static AgentDTO ToDto(this Agent agent)
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
            Rooms = agent.Rooms.Select(r => r.ToDto()).ToList(),
            Messages = agent.Messages.Select(m => m.ToDto()).ToList(),
        };
    }
}