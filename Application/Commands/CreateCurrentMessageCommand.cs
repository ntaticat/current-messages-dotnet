using Domain.Models;
using MediatR;
using Persistence;

namespace Application.Commands;

public class CreateCurrentMessageCommand
{
    public record CurrentMessageInfoCommand : IRequest
    {
        public Guid ChatMessageId { get; set; }
        public Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<CurrentMessageInfoCommand>
    {
        private readonly CurrentMessagesNetContext _context;

        public Handler(CurrentMessagesNetContext context)
        {
            _context = context;
        }
        
        public async Task<Unit> Handle(CurrentMessageInfoCommand request, CancellationToken cancellationToken)
        {
            var chatMessage = await _context.ChatMessages.FindAsync(request.ChatMessageId);

            if (chatMessage == null)
            {
                throw new Exception("No se encontró el mensaje");
            }

            var currentMessage = new CurrentMessage
            {
                MessageText = chatMessage.MessageText,
                UserId = request.UserId
            };

            await _context.CurrentMessages.AddAsync(currentMessage);
            
            var result = await _context.SaveChangesAsync();
            
            if (result > 0)
            {
                return Unit.Value;
            }
            
            throw new Exception("No se pudo crear el mensaje frecuente");
        }
    }
}