using PetShopProj.Models;

namespace PetShopProj.Hubs
{
    public interface ICallCenterHub
    {
        Task NewCallReceived(Call newCall);
        Task JoinCallCenters();
    }
}