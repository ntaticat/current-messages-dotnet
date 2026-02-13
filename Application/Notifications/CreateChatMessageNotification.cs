using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.ChatMessage;
using MediatR;

namespace Application.Notifications
{
    public record CreateChatMessageNotification(
        ChatMessageDto ChatMessage,
        Guid ChatId
    ) : INotification;
}