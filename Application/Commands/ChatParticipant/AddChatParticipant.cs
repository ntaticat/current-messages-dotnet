using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ChatParticipant;

public class AddChatParticipant
{
    public record Command(Guid chatId, Guid guestId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            
            if (userId is null)
            {
                throw new Exception("User not found");
            }
            
            var chat = await _context.Chats
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.ChatId == request.chatId, cancellationToken);
            
            if (chat == null)
                throw new Exception("Chat not found");
            
            chat.AddParticipant(userId.Value, request.guestId);
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}