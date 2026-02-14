using Application.Common.Exceptions;
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
                throw new UnauthorizedException("Usuario no autenticado");
            }
            
            var chat = await _context.Chats
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.ChatId == request.chatId, cancellationToken);
            
            if (chat == null)
                throw new NotFoundException("Chat no encontrado");
            
            chat.AddParticipant(userId.Value, request.guestId);
            
            var result = await _context.SaveChangesAsync(cancellationToken);
            
            if (result <= 0)
                throw new OperationFailedException("No se pudo agregar al participante al chat");
            
            return Unit.Value;
        }
    }
}