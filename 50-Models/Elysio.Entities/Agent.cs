namespace Elysio.Entities;

public class Agent
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Prompt { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Model { get; set; } = string.Empty;

    public string? Description { get; set; }
    public List<Room> Rooms { get; } = [];
    public List<Message> Messages { get; } = [];
}