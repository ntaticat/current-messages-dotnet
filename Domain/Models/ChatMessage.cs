namespace Domain.Models;

public class ChatMessage
{
    public Guid ChatMessageId { get; set; }
    public string MessageText { get; set; }
    public DateTime SentDate { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ChatOwnerId { get; set; }
    public Chat ChatOwner { get; set; }
}