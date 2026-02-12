using Application.Dtos.ChatMessage;
using Application.Dtos.User;

namespace Application.Dtos.Chat;

public class ChatDto
{
    public Guid ChatId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<UserDto>? Users { get; set; }
}