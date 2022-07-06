using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace szakdolgozatAPI_v02.Hubs
{
    public class NotificationHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected to SignalR.");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Caller.SendAsync("Disconnected from SignalR.");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
