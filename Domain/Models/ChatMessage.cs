using Domain.Exceptions;

namespace Domain.Models;

public class ChatMessage
{
    public Guid Id { get; private set; }
    // Ciphertext AES-256-GCM en Base64
    public string EncryptedText { get; private set; }
    // Vector de inicialización AES — único por mensaje, no es secreto,
    // pero sin él es imposible descifrar. También en Base64.
    public string Iv { get; private set; }
    public DateTime SentAt { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid ChatId { get; private set; }
    public Chat Chat { get; private set; }
    
    protected ChatMessage() { }
    
    public ChatMessage(Guid chatId, Guid userId, string encryptedText, string iv)
    {
        if (string.IsNullOrWhiteSpace(encryptedText))
            throw new BusinessRuleException("El texto cifrado no puede estar vacío");
        
        if (string.IsNullOrWhiteSpace(iv))
            throw new BusinessRuleException("El IV no puede estar vacío");
        
        Id = Guid.NewGuid();
        ChatId = chatId;
        UserId = userId;
        EncryptedText = encryptedText;
        Iv = iv;
        SentAt = DateTime.UtcNow;
    }
}