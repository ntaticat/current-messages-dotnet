using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Queries;

public class GetCurrentMessagesQuery
{
    public record CurrentMessagesQuery : IRequest<List<CurrentMessage>> {
        public string? UserId { get; set; }
    }

    public class Handler : IRequestHandler<CurrentMessagesQuery, List<CurrentMessage>>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }

        public async Task<List<CurrentMessage>> Handle(CurrentMessagesQuery request, CancellationToken cancellationToken)
        {
            List<CurrentMessage> currentMessages = new List<CurrentMessage>();

            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                currentMessages = await _context.CurrentMessages.ToListAsync();
            }
            else
            {
                currentMessages = await _context.CurrentMessages
                    .Where(currentMessages => currentMessages.UserId.ToString() == request.UserId).ToListAsync();
            }

            if (currentMessages == null)
            {
                throw new Exception("mensajes frecuentes no encontrados");
            }
            
            return currentMessages;
        }
    }
}