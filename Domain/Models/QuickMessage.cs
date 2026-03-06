using Domain.Exceptions;

namespace Domain.Models;

public class QuickMessage
{
    public Guid Id { get; private set; }
    public string Text { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    protected QuickMessage() { }

    public QuickMessage(Guid userId, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new BusinessRuleException("El texto del mensaje rápido no puede estar vacío.");
        
        Id = Guid.NewGuid();
        UserId = userId;
        Text = text;
    }
}