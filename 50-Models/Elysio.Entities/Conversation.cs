namespace Elysio.Entities;

public class Conversation
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public Guid? UpdaterId { get; set; }
    public User? Updater { get; set; }

    public string Name { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public List<Message> Messages { get; } = [];
}