using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Queries;

public class GetCurrentMessagesQuery
{
    public record CurrentMessagesQuery : IRequest<List<CurrentMessage>> {}

    public class Handler : IRequestHandler<CurrentMessagesQuery, List<CurrentMessage>>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }

        public async Task<List<CurrentMessage>> Handle(CurrentMessagesQuery request, CancellationToken cancellationToken)
        {
            var currentMessages = await _context.CurrentMessages.ToListAsync();
            
            if (currentMessages == null)
            {
                throw new Exception("mensajes frecuentes no encontrados");
            }
            
            return currentMessages;
        }
    }
}