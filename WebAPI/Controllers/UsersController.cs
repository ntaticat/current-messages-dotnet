using Application.Commands;
using Application.Commands.User;
using Application.Dtos.User;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.User;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet()]
    public async Task<ActionResult<UserDto>> GetUserProfile()
    {
        return await _mediator.Send(new GetUserProfile.Query());
    }
    
    [Authorize]
    [HttpGet("{userId:guid}/public-key")]
    public async Task<ActionResult> GetUserPublicKey(Guid userId)
    {
        var result = await _mediator.Send(new GetUserPublicKey.Query(userId));
        return Ok(new { publicKey = result });
    }
    
    [Authorize]
    [HttpPut("keys")]
    public async Task<Unit> PostUserKeys(RegisterUserKeysRequest request)
    {
        var command = new RegisterUserKeys.Command(request.PublicKey, request.EncryptedPrivateKey);
        return await _mediator.Send(command);
    }
}