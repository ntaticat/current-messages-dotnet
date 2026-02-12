using System.Collections.ObjectModel;

namespace Domain.Models;

public class User
{
    
    public Guid Id { get; private set; }
    public string FullName { get; private set; }
    
    private readonly List<ChatParticipant> _chats = new();
    private readonly List<QuickMessage> _quickMessages = new();
    
    public IReadOnlyCollection<ChatParticipant> Chats => _chats;
    public IReadOnlyCollection<QuickMessage> QuickMessages => _quickMessages;
    
    protected User() { }
    
    public User(Guid id, string fullName)
    {
        Id = id;
        FullName = fullName;
    }
}