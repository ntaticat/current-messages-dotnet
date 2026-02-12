using Application.Common.Interfaces;
using MediatR;

namespace Application.Commands.ChatMessage;

public class CreateChatMessage
{
    public record Command(Guid chatId, string text) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new Exception("User not found");
            }
            
            var chatMessage = new Domain.Models.ChatMessage(
                request.chatId, 
                userId.Value,
                request.text
            );

            await _context.ChatMessages.AddAsync(chatMessage, cancellationToken);

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("No se pudo crear el chatmessage");
            
            return Unit.Value;
        }
    }
}