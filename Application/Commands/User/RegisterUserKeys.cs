using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.User;

public class RegisterUserKeys
{
    public record Command(
        string PublicKey,
        string EncryptedPrivateKey
    ) : IRequest;

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
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
                ?? throw new NotFoundException("Usuario no encontrado");
            
            if(user.HasKeys)
                throw new ConflictException("El usuario ya tiene claves registradas");
            
            user.RegisterKeys(request.PublicKey, request.EncryptedPrivateKey);
            var result = await  _context.SaveChangesAsync(cancellationToken);
            
            if (result <= 0)
                throw new OperationFailedException("No se pudo crear el chat");
            
            return Unit.Value;
            
        }
    }
}