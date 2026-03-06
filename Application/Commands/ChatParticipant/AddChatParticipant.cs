using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ChatParticipant;

public class AddChatParticipant
{
    public record Command(Guid ChatId, Guid GuestId, string EncryptedRoomKeyForGuest) : IRequest;

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

            var guest = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.GuestId, cancellationToken);
            
            if (guest is null)
                throw new NotFoundException("El usuario a agregar no existe");
            
            if (!guest.HasKeys)
                throw new ValidationException("El usuario a agregar aún no tiene claves E2EE registradas");
            
            var chat = await _context.Chats
                .Include(c => c.Participants)
                .Include(c => c.KeyDistributions)
                .FirstOrDefaultAsync(c => c.Id == request.ChatId, cancellationToken);
            
            if (chat == null)
                throw new NotFoundException("Chat no encontrado");
            
            chat.AddParticipant(userId.Value, request.GuestId, request.EncryptedRoomKeyForGuest);
            
            var result = await _context.SaveChangesAsync(cancellationToken);
            
            if (result <= 0)
                throw new OperationFailedException("No se pudo agregar al participante al chat");
            
            return Unit.Value;
        }
    }
}