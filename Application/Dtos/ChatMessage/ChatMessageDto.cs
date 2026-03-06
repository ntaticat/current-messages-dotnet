namespace Application.Dtos.ChatMessage;

public class ChatMessageDto
{
    public Guid Id { get; set; }
    public string EncryptedText { get; set; }
    public string Iv { get; set; }
    public DateTime SentAt { get; set; }
    public Guid UserId { get; set; }
}