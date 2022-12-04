using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Queries;

public class GetChatsQuery
{
    public record ChatsQuery : IRequest<List<Chat>>
    {
        public Guid? UserId { get; set; }
    }

    public class Handler : IRequestHandler<ChatsQuery, List<Chat>>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> Handle(ChatsQuery request, CancellationToken cancellationToken)
        {
            List<Chat> chatList = new List<Chat>();
            var chats = await _context.Chats.Include(c => c.Users).ToListAsync();
            
            if (chats == null)
            {
                throw new Exception("chats no encontrados");
            }
            
            if (request.UserId != null)
            {
                var chatQueryList = chats.SelectMany(c => c.Users, (chat, user) => new { chat, user })
                    .Where(chatAndUser => chatAndUser.user.UserId == request.UserId).Select(chatAndUser => chatAndUser.chat).ToList();

                chatList = chatQueryList;
            }
            else
            {
                chatList = chats;
            }
            
            return chatList;
        }
    }
}