namespace WebAPI.Contracts.Chat;

public record AddChatParticipantRequest(
    Guid guestId,
    string encryptedRoomKeyForGuest
    );