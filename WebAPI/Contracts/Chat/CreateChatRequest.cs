namespace WebAPI.Contracts.Chat;

public record CreateChatRequest(
    string name,
    string encryptedRoomKey
);