using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.Notifications
{
    public record CreateChatMessageNotification : INotification
    {
        public Guid ChatMessageId { get; set; }
        public string MessageText { get; set; }
        public DateTime SentDate { get; set; }

        public Guid UserId { get; set; }
        public Guid ChatOwnerId { get; set; }
    }
}