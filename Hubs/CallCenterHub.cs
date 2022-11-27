using Microsoft.AspNetCore.SignalR;
using PetShopProj.Models;

namespace PetShopProj.Hubs
{
    public class CallCenterHub : Hub<ICallCenterHub>
    {
        public async Task NewCallReceived(Call newCall)
        {
            await Clients.Group("CallCenters").NewCallReceived(newCall);
        }

        public async Task JoinCallCenters()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "CallCenters");
            //Clients.Caller
        }
    }
}
