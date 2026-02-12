namespace WebAPI.Contracts.User;

public record LoginUserRequest(
    string Email,
    string Password
);