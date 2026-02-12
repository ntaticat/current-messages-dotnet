using Application.Commands.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.User;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<Guid> Register(RegisterUserRequest request)
    {
        var command = new RegisterUser.Command(request.FullName, request.Email, request.Password);
        var userId = await _mediator.Send(command);
        return userId;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var command = new LoginUser.Command(request.Email, request.Password);
        var token = await _mediator.Send(command);
        
        return Ok(new { token });
    }
}