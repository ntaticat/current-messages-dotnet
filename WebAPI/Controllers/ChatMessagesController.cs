using Application.Commands.ChatMessage;
using Application.Dtos.ChatMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.ChatMessage;

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
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Unit>> PostChatMessage([FromBody] CreateChatMessageRequest request)
    {
        var command = new CreateChatMessage.Command(request.ChatId, request.Message);
        return await _mediator.Send(command);
    }
}