using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.QuickMessage;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.QuickMessage;

public class GetMyQuickMessages
{
    public record Query() : IRequest<List<QuickMessageDto>>;

    public class Handler : IRequestHandler<Query, List<QuickMessageDto>>
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

        public async Task<List<QuickMessageDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                throw new UnauthorizedException("Usuario no autenticado");
            
            var quickMessages = await _context.QuickMessages
                .Where(currentMessages => currentMessages.UserId == userId.Value)
                .ProjectTo<QuickMessageDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (quickMessages == null)
                throw new NotFoundException("Quickmessages no encontrados");
            
            return quickMessages;
        }
    }
}