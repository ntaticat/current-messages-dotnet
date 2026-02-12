using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Notifications
{
    public record CreateChatMessageNotification(
        Guid ChatMessageId,
        string MessageText,
        DateTime SentDate,
        Guid UserId,
        Guid ChatOwnerId
    ) : INotification;
}