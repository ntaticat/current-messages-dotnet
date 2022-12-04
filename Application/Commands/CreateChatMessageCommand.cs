using Application.Hubs;
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
        public bool SetAsCurrentMessage { get; set; }
    }

    public class Handler : IRequestHandler<ChatMessageInfoCommand>
    {
        private readonly CurrentMessagesNetContext _context;
        private readonly IHubContext<ChatHub> _chatHub;

        public Handler(CurrentMessagesNetContext context, IHubContext<ChatHub> chatHub)
        {
            _context = context;
            _chatHub = chatHub;
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
            
            await _chatHub.Clients.All.SendAsync("SendNewChatMessage", chatMessage);
            
            if (!request.SetAsCurrentMessage)
            {
                return Unit.Value;
            }

            var currentMessage = new CurrentMessage
            {
                CurrentMessageId = chatMessage.ChatMessageId,
                UserId = chatMessage.UserId,
                MessageText = chatMessage.MessageText
            };
            
            await _context.CurrentMessages.AddAsync(currentMessage);
            
            var currentMessageResult = await _context.SaveChangesAsync();

            if (currentMessageResult > 0)
            {
                return Unit.Value;
            }
            
            throw new Exception("No se pudo registrar el chatmessage como currentmessage");
        }
    }
}