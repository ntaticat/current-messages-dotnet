using Application.Commands;
using Application.Queries;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Chat>>> GetChats()
    {
        return await _mediator.Send(new GetChatsQuery.ChatsQuery());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Chat>> GetChat(Guid id)
    {
        return await _mediator.Send(new GetChatQuery.ChatQuery { ChatId = id });
    }

    [HttpPost]
    public async Task<ActionResult<Unit>> PostChat([FromBody] CreateChatCommand.ChatInfoCommand data)
    {
        return await _mediator.Send(data);
    }
}