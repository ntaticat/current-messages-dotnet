namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> CreateUserAsync(string fullName, string email, string password);
    Task<string> LoginAsync(string email, string password);
}