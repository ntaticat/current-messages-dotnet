using System.Collections.ObjectModel;
using Domain.Exceptions;

namespace Domain.Models;

public class Chat
{
    public Guid ChatId { get; private set; }
    public string Name { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private readonly List<ChatParticipant> _participants = new();
    private readonly List<ChatMessage> _messages = new();
    
    public IReadOnlyCollection<ChatParticipant> Participants => _participants;
    public IReadOnlyCollection<ChatMessage> Messages => _messages;
    
    protected Chat() { }
    
    public Chat(Guid ownerId, string name = null)
    {
        ChatId = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
        
        _participants.Add(new ChatParticipant(ChatId, ownerId, ChatRole.Owner));
    }

    public void AddParticipant(Guid requesterId, Guid newUserId)
    {
        var requester = _participants
            .FirstOrDefault(p => p.UserId == requesterId);
        
        if (requester is null)
            throw new BusinessRuleException("El solicitante no está en el chat");

        if (requester.Role != ChatRole.Admin && requester.Role != ChatRole.Owner)
            throw new BusinessRuleException("El solicitante no puede añadir participantes al chat");

        if (_participants.Any(p => p.UserId == newUserId))
            throw new BusinessRuleException("El participante ya está en el chat");
        
        _participants.Add(new ChatParticipant(ChatId, newUserId, ChatRole.Member));
    }
}