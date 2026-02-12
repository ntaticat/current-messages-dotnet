namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Guid> CreateUserAsync(string fullName, string email, string password);
    Task<string> LoginAsync(string email, string password);
}