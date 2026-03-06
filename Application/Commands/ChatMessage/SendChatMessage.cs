using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.ChatMessage;
using Application.Notifications;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.ChatMessage;

public class SendChatMessage
{
    public record Command(Guid ChatId, string EncryptedText, string Iv) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public Handler(IApplicationDbContext context, ICurrentUserService currentUserService, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new UnauthorizedException("Usuario no autenticado");
            }
            
            var chat = await _context.Chats
                .Include(c => c.Participants)
                .FirstOrDefaultAsync(c => c.Id == request.ChatId, cancellationToken);
            
            if (chat == null)
                throw new NotFoundException("Chat no encontrado");
            
            var isParticipant = chat.Participants
                .Any(p => p.UserId == userId);
            
            if (!isParticipant)
                throw new ForbiddenException("Usuario no es parte del chat");
            
            var chatMessage = new Domain.Models.ChatMessage(
                request.ChatId, 
                userId.Value,
                request.EncryptedText,
                request.Iv
            );

            await _context.ChatMessages.AddAsync(chatMessage, cancellationToken);

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new OperationFailedException("No se pudo crear el chatmessage");
            
            var chatMessageDto = _mapper.Map<ChatMessageDto>(chatMessage);
            await _mediator.Publish(new CreateChatMessageNotification(chatMessageDto, request.ChatId), cancellationToken);
            
            return Unit.Value;
        }
    }
}