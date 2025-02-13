namespace Elysio.Entities;

public class Room
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<Agent> Agents { get; } = [];
    public List<Conversation> Conversations { get; } = [];
}