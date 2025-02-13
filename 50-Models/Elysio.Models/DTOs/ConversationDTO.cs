namespace Elysio.Models.DTOs;

public record ConversationDTO
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public string Name { get; set; } = string.Empty;
    public Guid RoomId { get; set; }
    public RoomDTO? Room { get; set; } = null!;
    public List<MessageDTO> Messages { get; set; } = [];
}