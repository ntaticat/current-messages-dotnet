using Application.Common.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.QuickMessage;

public class CreateQuickMessage
{
    public record Command(Guid ChatMessageId) : IRequest;

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
                throw new Exception("User not found");
            
            var chatMessage = await _context.ChatMessages
                .Include(m => m.Chat)
                .ThenInclude(c => c.Participants)
                .FirstOrDefaultAsync(m => m.ChatMessageId == request.ChatMessageId, cancellationToken);

            if (chatMessage == null)
                throw new Exception("No se encontró el mensaje");
            
            var isParticipant = chatMessage.Chat.Participants
                .Any(p => p.UserId == userId);
            
            if (!isParticipant)
                throw new UnauthorizedAccessException("You are not part of this chat");

            var quickMessage = new Domain.Models.QuickMessage(
                userId.Value,
                chatMessage.Text
            );

            await _context.QuickMessages.AddAsync(quickMessage, cancellationToken);
            var result = await _context.SaveChangesAsync(cancellationToken);
            
            if (result <= 0)
                throw new Exception("No se pudo crear el mensaje frecuente");
            
            return Unit.Value;
        }
    }
}