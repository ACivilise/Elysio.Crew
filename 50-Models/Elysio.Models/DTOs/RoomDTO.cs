namespace Elysio.Models.DTOs;

public record RoomDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Guid CreatorId { get; set; }
    public UserDTO? Creator { get; set; }
    public Guid? UpdaterId { get; set; }
    public UserDTO? Updater { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<AgentDTO> Agents { get; set; } = [];
    public List<ConversationDTO> Conversations { get; set; } = [];
}