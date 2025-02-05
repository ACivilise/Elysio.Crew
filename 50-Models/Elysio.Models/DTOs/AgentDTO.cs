namespace Elysio.Models.DTOs;

public record AgentDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public UserDTO? Creator { get; set; }
    public Guid? UpdaterId { get; set; }
    public UserDTO? Updater { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Model { get; set; } = string.Empty;

    public string? Description { get; set; }
    public List<RoomDTO> Rooms { get; set; } = [];
    public List<MessageDTO> Messages { get; set; } = [];
}