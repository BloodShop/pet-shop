using Microsoft.AspNetCore.SignalR;
using PetShopProj.Models;

namespace PetShopProj.Hubs
{
    public class CallCenterHub : Hub<ICallCenterHub>
    {
        public async Task NewCallReceivedAsync(Call newCall) => 
            await Clients.Group("CallCenters").NewCallReceivedAsync(newCall);

        public async Task CallDeletedAsync() =>
            await Clients.Group("CallCenters").CallDeletedAsync();

        public async Task CallEditedAsync(Call editCall) =>
            await Clients.Group("CallCenters").CallEditedAsync(editCall);

        public async Task JoinCallCenters() => 
            await Groups.AddToGroupAsync(Context.ConnectionId, "CallCenters");
    }
}
