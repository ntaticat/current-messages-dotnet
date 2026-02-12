namespace WebAPI.Contracts.User;

public record RegisterUserRequest(
    string FullName,
    string Email,
    string Password
);