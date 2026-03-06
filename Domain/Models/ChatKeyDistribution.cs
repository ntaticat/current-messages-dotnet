using Domain.Exceptions;

namespace Domain.Models;

public class ChatKeyDistribution
{
    public Guid ChatId { get; private set; }
    public Chat Chat { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    // La RoomKey AES-256 del chat, cifrada con la PublicKey RSA de este usuario.
    // El servidor no puede descifrarla porque no tiene ninguna PrivateKey.
    public string EncryptedRoomKey { get; private set; }
    // Cuándo se distribuyó (útil para auditoría y para fase 2 con rotación de claves)
    public DateTime DistributedAt { get; private set; }

    protected ChatKeyDistribution() { }
    
    public ChatKeyDistribution(Guid chatId, Guid userId, string encryptedRoomKey)
    {
        ChatId = chatId;
        UserId = userId;
        EncryptedRoomKey = encryptedRoomKey;
        DistributedAt = DateTime.UtcNow;
    }

    public void Reencrypt(string newEncryptedRoomKey)
    {
        if (string.IsNullOrWhiteSpace(newEncryptedRoomKey))
            throw new BusinessRuleException("La nueva clave cifrada no puede estar vacía");
        EncryptedRoomKey = newEncryptedRoomKey;
        DistributedAt = DateTime.UtcNow;
    }
}