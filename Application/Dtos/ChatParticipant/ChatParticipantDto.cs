using Domain.Models;

namespace Application.Dtos.ChatParticipant;

public class ChatParticipantDto
{
    public Guid Id { get; set; }
    public string FullName {get; set; }
    public bool HasKeys { get; set; }
    public string Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? LastReadAt { get; set; }
}