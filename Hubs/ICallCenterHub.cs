using PetShopProj.Models;

namespace PetShopProj.Hubs
{
    public interface ICallCenterHub
    {
        Task NewCallReceivedAsync(Call newCall);
        Task JoinCallCenters();
        Task CallDeletedAsync();
        Task CallEditedAsync(Call editCall);
    }
}