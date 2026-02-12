using Application.Commands.Chat;
using Application.Commands.ChatParticipant;
using Application.Dtos.Chat;
using Application.Dtos.ChatMessage;
using Application.Queries.Chat;
using Application.Queries.ChatMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.Chat;

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
    
    [Authorize]
    [HttpGet()]
    public async Task<ActionResult<List<ChatDto>>> GetChatsByUserId()
    {
        return await _mediator.Send(new GetMyChats.Query());
    }
    
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChatDto>> GetChat(Guid id)
    {
        return await _mediator.Send(new GetChat.Query(id));
    }
    
    [Authorize]
    [HttpGet("{id:guid}/messages")]
    public async Task<List<ChatMessageDto>> GetChatMessages(Guid id, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        return await _mediator.Send(new GetChatMessages.Query(id, page, pageSize));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Unit>> PostChat([FromBody] CreateChatRequest request)
    {
        var command = new CreateChat.Command(request.name);
        return await _mediator.Send(command);
    }
    
    [Authorize]
    [HttpPost("{id:guid}/participants")]
    public async Task<ActionResult<Unit>> PostChatParticipant(Guid id, [FromBody] AddChatParticipantRequest request)
    {
        var command = new AddChatParticipant.Command(id, request.guestId);
        return await _mediator.Send(command);
    }
}