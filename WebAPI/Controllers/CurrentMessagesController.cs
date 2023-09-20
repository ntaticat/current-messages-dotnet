using Application.Commands;
using Application.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrentMessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrentMessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<CurrentMessage>>> GetCurrentMessages(string? userId = "")
    {
        return await _mediator.Send(new GetCurrentMessagesQuery.CurrentMessagesQuery {UserId = userId});
    }
    
    [HttpPost]
    public async Task<ActionResult<Unit>> PostCurrentMessage([FromBody] CreateCurrentMessageCommand.CurrentMessageInfoCommand data)
    {
        return await _mediator.Send(data);
    }
}