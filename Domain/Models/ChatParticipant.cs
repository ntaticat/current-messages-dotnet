namespace Domain.Models;

public enum ChatRole
{
    Owner = 0,
    Admin = 1,
    Member = 2
}

public class ChatParticipant
{
    public Guid ChatId { get; private set; }
    public Chat Chat { get; private set; }
    
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    
    public ChatRole Role { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public DateTime? LastReadAt { get; private set; }

    protected ChatParticipant() { }

    public ChatParticipant(Guid chatId, Guid userId, ChatRole role)
    {
        ChatId = chatId;
        UserId = userId;
        JoinedAt = DateTime.UtcNow;
        Role = role;
    }
    
    public void MarkAsRead()
    {
        LastReadAt = DateTime.UtcNow;
    }
    
    public void PromoteToAdmin()
    {
        Role = ChatRole.Admin;
    }
}