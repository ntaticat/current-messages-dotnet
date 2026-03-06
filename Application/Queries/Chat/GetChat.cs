using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Chat;
using Application.Dtos.ChatParticipant;
using Application.Dtos.User;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

public class GetChat
{
    public record Query(Guid ChatId) : IRequest<ChatDto>;

    public class Handler : IRequestHandler<Query, ChatDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<ChatDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
                throw new UnauthorizedException("Usuario no autenticado");
            
            var chat = await _context.Chats
                .AsNoTracking()
                .Where(chat => chat.Id == request.ChatId &&
                               chat.Participants.Any(p => p.UserId == userId.Value))
                .Select(c => new ChatDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    CreatedAt = c.CreatedAt,
                    HasRoomKey = c.KeyDistributions.Any(k => k.UserId == userId.Value),
                    Participants = c.Participants
                        .Select(p => new ChatParticipantDto
                        {
                            Id = p.User.Id,
                            FullName = p.User.FullName,
                            HasKeys = p.User.HasKeys,
                            Role = p.Role.ToString(),
                            JoinedAt = p.JoinedAt,
                            LastReadAt = p.LastReadAt
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (chat == null)
                throw new NotFoundException("Chat no encontrado");
            
            return chat;
        }
    }
}