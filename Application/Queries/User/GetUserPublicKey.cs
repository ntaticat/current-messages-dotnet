using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User;

public class GetUserPublicKey
{
    public record Query(Guid UserId) : IRequest<string>;

    public class Handler : IRequestHandler<Query, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<string> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
                throw new UnauthorizedException("Usuario no autenticado");

            var publicKey = await _context.Users
                .Where(u => u.Id == request.UserId)
                .Select(u => u.PublicKey)
                .FirstOrDefaultAsync(cancellationToken);

            if (publicKey is null)
                throw new NotFoundException("Usuario no encontrado o sin claves registradas");

            return publicKey;
        }
    }
}