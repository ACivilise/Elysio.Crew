using Elysio.Models.Enums;

namespace Elysio.Entities;

public class Message
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public User? Creator { get; set; }
    public Guid? UpdaterId { get; set; }
    public User? Updater { get; set; }

    public RolesEnum Role { get; set; }
    public string Content { get; set; } = string.Empty;

    public Guid? AgentId { get; set; }
    public Agent? Agent { get; set; } = null!;

    public Guid ConversationId { get; set; }
    public Conversation Conversation { get; set; } = null!;
}