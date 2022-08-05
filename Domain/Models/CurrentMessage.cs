namespace Domain.Models;

public class CurrentMessage
{
    public Guid CurrentMessageId { get; set; }
    public string MessageText { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}