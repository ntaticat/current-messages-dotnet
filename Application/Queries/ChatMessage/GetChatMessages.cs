using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.ChatMessage;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.ChatMessage;

public class GetChatMessages
{
    public record Query(Guid ChatId, int Page = 1, int PageSize = 50) : IRequest<List<ChatMessageDto>>;
    
    public class Handler : IRequestHandler<Query, List<ChatMessageDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<List<ChatMessageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
                throw new UnauthorizedException("Usuario no autenticado");
            
            var isParticipant = await _context.ChatParticipants
                .AsNoTracking()
                .AnyAsync(p =>
                        p.ChatId == request.ChatId &&
                        p.UserId == userId,
                    cancellationToken);
            
            if (!isParticipant)
                throw new ForbiddenException("Usuario no es miembro del chat");
            
            var messages = await _context.ChatMessages
                .AsNoTracking()
                .Where(m => m.ChatId == request.ChatId)
                .OrderByDescending(m => m.SentAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<ChatMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
            
            return messages;
        }
    }
}