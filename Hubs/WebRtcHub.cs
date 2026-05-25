using Microsoft.AspNetCore.SignalR;
using ZoZoom.Services;

namespace ZoZoom.Hubs
{
    public class WebRtcHub : Hub
    {
        private readonly SfuConnectionManager _sfuManager;

        public WebRtcHub(SfuConnectionManager sfuManager)
        {
            _sfuManager = sfuManager;
        }
       
        public override Task OnConnectedAsync()
        {
            // Регистрируем sender для этого конкретного connectionId
            _sfuManager.RegisterSender(Context.ConnectionId, async (sdp) =>
            {
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOffer", sdp);
            });

            return base.OnConnectedAsync();
        }

        public async Task SendOffer(string sdpOffer)
        {
            var answer = await _sfuManager.ProcessSdpOffer(Context.ConnectionId, sdpOffer);
            await Clients.Caller.SendAsync("ReceiveAnswer", answer);
        }

        public Task SendAnswer(string sdpAnswer)
        {
            _sfuManager.ProcessSdpAnswer(Context.ConnectionId, sdpAnswer);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _sfuManager.UnregisterSender(Context.ConnectionId);
            _sfuManager.RemovePeer(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}