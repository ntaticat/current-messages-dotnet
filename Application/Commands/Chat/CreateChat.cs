using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Commands.Chat;

public class CreateChat
{
    public record Command(string name) : IRequest;

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

            if (userId == null)
            {
                throw new UnauthorizedException("Usuario no autenticado");
            }
            
            var chat = new Domain.Models.Chat(userId.Value, request.name);
            
            await _context.Chats.AddAsync(chat, cancellationToken);
            
            var result = await _context.SaveChangesAsync(cancellationToken);
            
            if (result <= 0)
                throw new OperationFailedException("No se pudo crear el chat");
            
            return Unit.Value;
        }
    }
}