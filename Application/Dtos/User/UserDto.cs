namespace Application.Dtos.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public bool HasKeys { get; set; }
    public string? PublicKey { get; set; }
    public string? EncryptedPrivateKey { get; set; }
}