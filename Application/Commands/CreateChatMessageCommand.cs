using Application.Notifications;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Persistence;

namespace Application.Commands;

public class CreateChatMessageCommand
{
    public record ChatMessageInfoCommand : IRequest
    {
        public string MessageText { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatOwnerId { get; set; }
    }

    public class Handler : IRequestHandler<ChatMessageInfoCommand>
    {
        private readonly CurrentMessagesNetContext _context;
        private readonly IMediator _mediator;

        public Handler(CurrentMessagesNetContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        
        public async Task<Unit> Handle(ChatMessageInfoCommand request, CancellationToken cancellationToken)
        {
            var chatMessage = new ChatMessage
            {
                MessageText = request.MessageText,
                UserId = request.UserId,
                ChatOwnerId = request.ChatOwnerId,
                SentDate = DateTime.Now
            };

            await _context.ChatMessages.AddAsync(chatMessage);

            var chatMessageResult = await _context.SaveChangesAsync();

            if (chatMessageResult <= 0)
            {
                throw new Exception("No se pudo crear el chatmessage");
            }
            
            await _mediator.Publish(
                new CreateChatMessageNotification
                {
                    ChatOwnerId = request.ChatOwnerId,
                    UserId = request.UserId,
                    MessageText = request.MessageText,
                    SentDate = DateTime.UtcNow,
                    ChatMessageId = chatMessage.ChatMessageId,
                }, 
                cancellationToken
            );
            
            return Unit.Value;
        }
    }
}