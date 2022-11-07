using Microsoft.EntityFrameworkCore;
using PetShopProj.Data;

namespace PetShopProj.Repositories
{
    public class Repository
    {
        readonly PetDbContext _context;
        public Repository(PetDbContext context) => _context = context;

        #region Cities repo
        //IEnumerable<City> IRepository<City>.GetAll() => _context.Cities!;
        //void IRepository<City>.Delete(int id)
        //{
        //    var city = _context.Cities!.Single(c => c.CityId == id);
        //    _context.Cities!.Remove(city);
        //    _context.SaveChanges();
        //}
        //void IRepository<City>.Insert(City city)
        //{
        //    _context.Cities!.Add(city);
        //    _context.SaveChanges();
        //}
        //void IRepository<City>.Update(int id, City city)
        //{
        //    var cityInDb = _context.Cities!.Single(c => c.CityId == id);
        //    cityInDb.Name = city.Name;
        //    _context.SaveChanges();
        //}
        #endregion
    }
}
