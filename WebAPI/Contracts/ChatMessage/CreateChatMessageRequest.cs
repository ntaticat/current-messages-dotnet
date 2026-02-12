namespace WebAPI.Contracts.ChatMessage;

public record CreateChatMessageRequest(
    Guid ChatId,
    string Message
);