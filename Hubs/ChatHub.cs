using Microsoft.AspNetCore.SignalR;

namespace PetShopProj.Hubs
{
    public class ChatHub : Hub
    {
        public async Task MessageAll(string sender, string message)
        {
            await Clients.All.SendAsync("NewMessage", sender, message);
        }
    }
}
