using System.Collections.ObjectModel;
using Domain.Exceptions;

namespace Domain.Models;

public class Chat
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private readonly List<ChatParticipant> _participants = new();
    private readonly List<ChatMessage> _messages = new();
    private readonly List<ChatKeyDistribution> _keyDistributions = new();
    
    public IReadOnlyCollection<ChatParticipant> Participants => _participants;
    public IReadOnlyCollection<ChatMessage> Messages => _messages;
    public IReadOnlyCollection<ChatKeyDistribution> KeyDistributions => _keyDistributions;
    
    protected Chat() { }
    
    public Chat(Guid userId, string name, string ownerEncryptedRoomKey)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
        
        _participants.Add(new ChatParticipant(Id, userId, ChatRole.Owner));
        _keyDistributions.Add(new ChatKeyDistribution(Id, userId, ownerEncryptedRoomKey));
    }

    public void AddParticipant(Guid requesterId, Guid newUserId, string encryptedRoomKeyForNewUser)
    {
        var requester = _participants
            .FirstOrDefault(p => p.UserId == requesterId);
        
        if (requester is null)
            throw new BusinessRuleException("El solicitante no está en el chat");

        if (requester.Role != ChatRole.Admin && requester.Role != ChatRole.Owner)
            throw new BusinessRuleException("El solicitante no puede añadir participantes al chat");

        if (_participants.Any(p => p.UserId == newUserId))
            throw new BusinessRuleException("El participante ya está en el chat");
        
        if (string.IsNullOrWhiteSpace(encryptedRoomKeyForNewUser))
            throw new BusinessRuleException("Debes proporcionar la clave del chat cifrada para el nuevo participante");
        
        _participants.Add(new ChatParticipant(Id, newUserId, ChatRole.Member));
        _keyDistributions.Add(new ChatKeyDistribution(Id, newUserId, encryptedRoomKeyForNewUser));
    }
    
    public void RemoveParticipant(Guid requesterId, Guid targetUserId)
    {
        var requester = _participants.FirstOrDefault(p => p.UserId == requesterId);
        
        if (requester is null)
            throw new BusinessRuleException("El solicitante no está en el chat");

        if (requester.Role != ChatRole.Admin && requester.Role != ChatRole.Owner)
            throw new BusinessRuleException("Sin permisos para eliminar participantes");

        var target = _participants.FirstOrDefault(p => p.UserId == targetUserId)
                     ?? throw new BusinessRuleException("El usuario no está en el chat");

        _participants.Remove(target);
        
        // La distribución de clave de este usuario ya no es válida.
        // NOTA: en fase 2 aquí rotarías la RoomKey para todos los demás (forward secrecy).
        var keyDist = _keyDistributions.FirstOrDefault(k => k.UserId == targetUserId);
        if (keyDist is not null)
            _keyDistributions.Remove(keyDist);
    }
}