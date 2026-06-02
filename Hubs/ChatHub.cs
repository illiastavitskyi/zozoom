using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ZoZoom.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("SystemMessage", $"{Context.User?.Identity?.Name} left {groupName}");
        }

        public async Task SendGroupMessage(string groupName, string message, string? fileUrl)
        {
            var sender = Context.User?.Identity?.Name ?? "Unknown";
            await Clients.OthersInGroup(groupName).SendAsync("ReceiveMessage", sender, message, fileUrl, groupName);
        }

        public async Task SendPrivateMessage(string targetUserId, string message, string? fileUrl)
        {
            var sender = Context.User?.Identity?.Name ?? "Unknown";
            await Clients.User(targetUserId).SendAsync("ReceivePrivateMessage", sender, message, fileUrl);
        }
    }
}
