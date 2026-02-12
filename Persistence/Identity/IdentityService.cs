using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Common.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public IdentityService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration)
    {
        _userManager = userManager;
        _context = context;
        _configuration = configuration;
    }
    
    public async Task<Guid> CreateUserAsync(string fullName, string email, string password)
    {
        if (await _userManager.FindByEmailAsync(email) != null)
            throw new Exception("El email ya está registrado");

        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        var identityUser = new ApplicationUser
        {
            UserName = email,
            Email = email
        };
        
        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
            throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
        
        var domainUser = new User(identityUser.Id, fullName);
        
        await _context.Users.AddAsync(domainUser);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        
        return identityUser.Id;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales invalidas");
        }
        
        var validPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!validPassword)
        {
            throw new UnauthorizedAccessException("Credenciales invalidas");
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Usa 'sub' en lugar de NameIdentifier
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Recomendado para unicidad
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(
            int.Parse(_configuration["Jwt:DurationInMinutes"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}