using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Chat;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Chat;

public class GetChats
{
    public record Query : IRequest<List<ChatDto>>;

    public class Handler : IRequestHandler<Query, List<ChatDto>>
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

        public async Task<List<ChatDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
                throw new UnauthorizedException("Usuario no autenticado");
            
            var chats = await _context.Chats.ProjectTo<ChatDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            
            return chats;
        }
    }
}