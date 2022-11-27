using PetShopProj.Data;
using PetShopProj.Models;

namespace PetShopProj.Repositories
{
    public class PetRepository : IRepository
    {
        readonly PetDbContext _ctx;
        public PetRepository(PetDbContext context) => _ctx = context;

        public void AddAnimal(Animal animal)
        {
            if (!_ctx.Animals!.Any(a => a.Name == animal.Name))
            {
                _ctx.Animals!.Add(animal);
                SaveChanges();
            }
        }

        IEnumerable<Animal> GetAnimals() => _ctx.Animals!;

        public IEnumerable<Animal> GetMostPopularAnimals(int count) =>
            GetAnimals().OrderBy(a => -a.Comments?.Count).Take(count);

        public IEnumerable<Category> GetCategory(string categoryName = "All")
        {
            var categories = _ctx.Categories!;
            return (categoryName == "All") ? categories : categories.Where(c => c.Name == categoryName);
        }

        public Animal? GetAnimal(int id) => _ctx.Animals!.FirstOrDefault(a => a.Id == id);

        public void AddComment(int animalId, string comment)
        {
            var animal = GetAnimal(animalId);
            if (animal != null)
            {
                animal.Comments!.Add(new Comment { Content = comment });
                SaveChanges();
            }
        }

        public Category? GetCategoryById(int id) => _ctx.Categories!.FirstOrDefault(c => c.Id == id);

        public void DeleteAnimal(int id)
        {
            var animal = GetAnimal(id);
            if (animal != null)
            {
                foreach (var comment in animal.Comments!)
                    _ctx.Comments!.Remove(comment);

                _ctx.Animals!.Remove(animal);
            }
            SaveChanges();
        }

        public void SaveChanges() => _ctx.SaveChanges();

        public IEnumerable<Animal> SearchAnimals(string text) =>
            _ctx.Animals!.Where(a => a.Name!.ToUpper().Contains(text.ToUpper()));

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category != null && category!.Animals!.Count == 0)
            {
                _ctx.Categories!.Remove(category);
                SaveChanges();
            }
        }

        public void AddCategory(Category newCategory)
        {
            if (_ctx.Categories!.Any(c => c.Name == newCategory.Name)) return;

            _ctx.Categories!.Add(newCategory);
            SaveChanges();
        }

        public void AddCall(Call model)
        {
            if (model != null)
                _ctx.Add(model);            
        }

        public Task<int> SaveChangesAsync() => _ctx.SaveChangesAsync();
    }
}