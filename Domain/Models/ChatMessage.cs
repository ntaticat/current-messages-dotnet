using Domain.Exceptions;

namespace Domain.Models;

public class ChatMessage
{
    public Guid ChatMessageId { get; private set; }
    public string Text { get; private set; }
    public DateTime SentAt { get; private set; }

    public Guid SenderId { get; private set; }
    public User Sender { get; private set; }
    public Guid ChatId { get; private set; }
    public Chat Chat { get; private set; }
    
    protected ChatMessage() { }
    
    public ChatMessage(Guid chatId, Guid senderId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new BusinessRuleException("El texto del mensaje no puede estar vacío");
        
        ChatMessageId = Guid.NewGuid();
        ChatId = chatId;
        SenderId = senderId;
        Text = text;
        SentAt = DateTime.UtcNow;
    }
}