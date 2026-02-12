using Application.Common.Interfaces;
using Application.Dtos.Chat;
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
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ChatDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new UnauthorizedAccessException("User not authenticated");
            }
            
            var chat = await _context.Chats
                .AsNoTracking()
                .Where(chat => chat.ChatId == request.ChatId &&
                               chat.Participants.Any(p => p.UserId == userId.Value))
                .ProjectTo<ChatDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (chat == null)
            {
                throw new Exception("Chat no encontrado");
            }
            
            return chat;
        }
    }
}