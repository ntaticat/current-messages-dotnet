using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

public class GetMyRoomKey
{
    public record Query(Guid ChatId) : IRequest<string>;
    
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
            var userId = _currentUserService.UserId
                         ?? throw new UnauthorizedException("Usuario no autenticado");

            var keyDistribution = await _context.ChatKeyDistributions
                                      .FirstOrDefaultAsync(
                                          ck => ck.ChatId == request.ChatId && ck.UserId == userId,
                                          cancellationToken)
                                  ?? throw new NotFoundException("No tienes clave para este chat");

            return keyDistribution.EncryptedRoomKey;
        }
    }
}