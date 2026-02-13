using Application.Commands;
using Application.Commands.User;
using Application.Dtos.User;
using Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
}