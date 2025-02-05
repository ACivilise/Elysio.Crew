using Elysio.Models.Enums;

namespace Elysio.Models.DTOs;

public record MessageDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public UserDTO? Creator { get; set; }
    public Guid? UpdaterId { get; set; }
    public UserDTO? Updater { get; set; }

    public RolesEnum Role { get; set; }
    public string Content { get; set; } = string.Empty;

    public Guid? AgentId { get; set; }
    public AgentDTO? Agent { get; set; } = null!;

    public Guid ConversationId { get; set; }
    public ConversationDTO? Conversation { get; set; } = null!;
}