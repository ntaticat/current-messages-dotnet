namespace Application.Dtos.ChatMessage;

public class ChatMessageDto
{
    public Guid ChatMessageId { get; set; }
    public string Text { get; set; }
    public DateTime SentAt { get; set; }
    public Guid SenderId { get; set; }
}