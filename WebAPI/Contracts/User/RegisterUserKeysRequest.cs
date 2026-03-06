namespace WebAPI.Contracts.User;

public record RegisterUserKeysRequest(
    string PublicKey,
    string EncryptedPrivateKey
);