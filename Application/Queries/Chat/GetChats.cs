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

        public Handler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ChatDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var chats = await _context.Chats.ProjectTo<ChatDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            
            if (chats == null)
            {
                throw new Exception("chats no encontrados");
            }
            
            return chats;
        }
    }
}