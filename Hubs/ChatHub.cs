using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using ZoZoom.Data;

namespace ZoZoom.Hubs
{
    [Authorize] // Ensure only logged-in users can connect
    public class ChatHub : Hub
    {
        // Group Chat
        public async Task SendGroupMessage(string groupName, string message, string fileUrl = null)
        {
            var senderName = Context.User.Identity.Name;
            await Clients.Group(groupName).SendAsync("ReceiveMessage", senderName, message, fileUrl, groupName);
        }

        // Private Chat
        public async Task SendPrivateMessage(string receiverUserId, string message, string fileUrl = null)
        {
            var senderName = Context.User.Identity.Name;
            // SignalR automatically routes to the specific user's connection by their Identity User ID
            await Clients.User(receiverUserId).SendAsync("ReceivePrivateMessage", senderName, message, fileUrl);
        }

        // Join a meeting/group room
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User.Identity.Name} joined {groupName}");
        }
    }
}