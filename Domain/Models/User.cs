using System.Collections.ObjectModel;
using Domain.Exceptions;

namespace Domain.Models;

public class User
{
    
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    
    // E2EE
    public string? PublicKey { get; private set; } // Base 64, sin cifrar
    public string? EncryptedPrivateKey { get; private set; } // Base 64, cifrado
    public bool HasKeys => PublicKey is not null;
    
    private readonly List<ChatParticipant> _chats = new();
    private readonly List<QuickMessage> _quickMessages = new();
    private readonly List<ChatKeyDistribution> _keyDistributions = new();
    
    public IReadOnlyCollection<ChatParticipant> Chats => _chats;
    public IReadOnlyCollection<QuickMessage> QuickMessages => _quickMessages;
    public IReadOnlyCollection<ChatKeyDistribution> KeyDistributions => _keyDistributions;
    
    
    protected User() { }
    
    public User(Guid id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }
    
    public void RegisterKeys(string publicKey, string encryptedPrivateKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
            throw new BusinessRuleException("La clave pública no puede estar vacía");
        
        if (string.IsNullOrWhiteSpace(encryptedPrivateKey))
            throw new BusinessRuleException("La clave privada cifrada no puede estar vacía");

        PublicKey = publicKey;
        EncryptedPrivateKey = encryptedPrivateKey;
    }
    
    public void UpdateKeys(string publicKey, string encryptedPrivateKey)
    {
        RegisterKeys(publicKey, encryptedPrivateKey);
    }
}