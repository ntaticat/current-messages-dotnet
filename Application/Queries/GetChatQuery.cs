using System.Net;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Queries;

public class GetChatQuery
{
    public record ChatQuery : IRequest<Chat> 
    {
        public Guid ChatId { get; set; }
    }

    public class Handler : IRequestHandler<ChatQuery, Chat>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }

        public async Task<Chat> Handle(ChatQuery request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.ChatId == request.ChatId);

            if (chat == null)
            {
                throw new Exception("Chat no encontrado");
            }
            
            return chat;
        }
    }
}