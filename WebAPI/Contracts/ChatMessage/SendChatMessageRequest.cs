namespace WebAPI.Contracts.ChatMessage;

public record SendChatMessageRequest(
    Guid ChatId,
    string encryptedText, 
    string iv
);