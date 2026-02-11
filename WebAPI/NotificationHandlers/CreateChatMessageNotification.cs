using Application.Notifications;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;

namespace WebAPI.NotificationHandlers
{
    public class CreateChatMessageNotificationHandler : INotificationHandler<CreateChatMessageNotification>
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public CreateChatMessageNotificationHandler(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Handle(CreateChatMessageNotification notification, CancellationToken cancellationToken)
        {
            await _hubContext.Clients.Group(notification.ChatOwnerId.ToString())
            .SendAsync("MessageReceived", new
            {
                notification.ChatMessageId,
                notification.UserId,
                notification.MessageText,
                notification.SentDate
            }, cancellationToken);
        }
    }
}