using System.Collections.ObjectModel;

namespace Domain.Models;

public class User
{
    public User()
    {
        Chats = new Collection<Chat>();
        CurrentMessages = new Collection<CurrentMessage>();
    }
    
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public virtual Collection<Chat> Chats { get; set; }
    public virtual ICollection<CurrentMessage> CurrentMessages { get; set; }
}