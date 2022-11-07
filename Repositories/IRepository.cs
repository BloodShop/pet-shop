namespace PetShopProj.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Delete(int id);
        void Update(int id, T entity);
        void Insert(T entity);
    }
}