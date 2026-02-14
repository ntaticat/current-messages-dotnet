using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.User;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.User;

public class GetUserProfile
{
    public record Query() : IRequest<UserDto>;

    public class Handler : IRequestHandler<Query, UserDto>
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
        
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
                throw new UnauthorizedException("Usuario no autenticado");

            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (user == null)
                throw new NotFoundException("Usuario no encontrado");
            
            return user;
        }
    }
}