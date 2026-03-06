using Application.Dtos.ChatMessage;
using Application.Dtos.ChatParticipant;
using Application.Dtos.User;

namespace Application.Dtos.Chat;

public class ChatDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<ChatParticipantDto>? Participants { get; set; }
    public bool HasRoomKey { get; set; }
}