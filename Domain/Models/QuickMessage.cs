using Domain.Exceptions;

namespace Domain.Models;

public class QuickMessage
{
    public Guid QuickMessageId { get; private set; }
    public string Text { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    protected QuickMessage() { }

    public QuickMessage(Guid userId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new BusinessRuleException("El texto del mensaje rápido no puede estar vacío.");
        
        QuickMessageId = Guid.NewGuid();
        UserId = userId;
        Text = text;
    }
}