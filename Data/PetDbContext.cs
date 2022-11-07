using Microsoft.EntityFrameworkCore;

namespace PetShopProj.Data
{
    public class PetDbContext : DbContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options)
        {
        }

    }
}
