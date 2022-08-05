using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Queries;

public class GetChatsQuery
{
    public record ChatsQuery : IRequest<List<Chat>> {}

    public class Handler : IRequestHandler<ChatsQuery, List<Chat>>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> Handle(ChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = await _context.Chats.ToListAsync();
            
            if (chats == null)
            {
                throw new Exception("Chats no encontrados");
            }
            
            return chats;
        }
    }
}