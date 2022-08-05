using Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs;

public class ChatHub : Hub
{
    public async Task SendNewChatMessage(ChatMessage chatMessage)
        => await Clients.All.SendAsync("SendNewChatMessage", chatMessage);
}