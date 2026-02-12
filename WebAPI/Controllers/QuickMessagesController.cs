using Application.Commands.QuickMessage;
using Application.Dtos.QuickMessage;
using Application.Queries.QuickMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Contracts.QuickMessage;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuickMessagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuickMessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<QuickMessageDto>>> GetQuickMessagesByUser()
    {
        return await _mediator.Send(new GetMyQuickMessages.Query());
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Unit>> PostQuickMessage([FromBody] CreateQuickMessageRequest request)
    {
        var command = new CreateQuickMessage.Command(request.ChatMessageId);
        return await _mediator.Send(command);
    }
}