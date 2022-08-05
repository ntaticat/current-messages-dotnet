using System.Collections.ObjectModel;
using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Commands;

public class CreateChatCommand
{
    public record ChatInfoCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
    }

    public class Handler : IRequestHandler<ChatInfoCommand>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(ChatInfoCommand request, CancellationToken cancellationToken)
        {
            var chat = new Chat();

            var userData = await _context.Users.FindAsync(request.UserId);
            
            if (userData == null)
            {
                throw new Exception($"No se encontró el usuario con el ID {request.UserId}");
            }
            
            var friendData = await _context.Users.FindAsync(request.FriendId);
            
            if (friendData == null)
            {
                throw new Exception($"No se encontró el usuario con el ID {request.FriendId}");
            }
            
            chat.Users.Add(userData);
            chat.Users.Add(friendData);
            
            await _context.Chats.AddAsync(chat);
            
            var result = await _context.SaveChangesAsync();
            
            if (result <= 0)
            {
                throw new Exception("No se pudo crear el chat");
            }
            
            return Unit.Value;
        }
    }
}