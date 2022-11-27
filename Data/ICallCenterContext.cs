using Microsoft.EntityFrameworkCore;
using PetShopProj.Models;

namespace PetShopProj.Data
{
    public interface ICallCenterContext
    {
        DbSet<Call> Calls { get; set; }
    }
}