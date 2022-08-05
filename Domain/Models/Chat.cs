using System.Collections.ObjectModel;

namespace Domain.Models;

public class Chat
{
    public Chat()
    {
        Users = new Collection<User>();
        Messages = new Collection<ChatMessage>();
    }
    public Guid ChatId { get; set; }
    public virtual Collection<User> Users { get; set; }
    public virtual Collection<ChatMessage> Messages { get; set; }
}