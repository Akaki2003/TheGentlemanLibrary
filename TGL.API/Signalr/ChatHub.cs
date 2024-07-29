using Microsoft.AspNetCore.SignalR;

namespace TGL.API.Signalr
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message) => await Clients.Caller.SendAsync("ReceiveMessage", user, message);

        public async void Test()
        {
        }

    }
}
