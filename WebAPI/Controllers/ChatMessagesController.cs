using Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatMessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost]
    public async Task<ActionResult<Unit>> PostChatMessage([FromBody] CreateChatMessageCommand.ChatMessageInfoCommand data)
    {
        return await _mediator.Send(data);
    }
}