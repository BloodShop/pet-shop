using Microsoft.EntityFrameworkCore;
using PetShopProj.Models;

namespace PetShopProj.Data
{
    public interface ICallCenter
    {
        DbSet<Call> Calls { get; set; }
    }
}