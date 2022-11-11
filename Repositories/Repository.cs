using PetShopProj.Data;
using PetShopProj.Models;

namespace PetShopProj.Repositories
{
    public class PetRepository : IRepository
    {
        readonly PetDbContext _context;
        public PetRepository(PetDbContext context) => this._context = context;

        public void AddAnimal(Animal animal)
        {
            _context.Animals!.Add(animal);
            SaveChanges();
        }

        public IEnumerable<Animal> GetAnimals() => _context.Animals!;

        public IEnumerable<Animal> GetMostPopularAnimals(int count) => 
            GetAnimals().OrderBy(a => -a.Comments?.Count).Take(count);

        public IEnumerable<Category> GetCategory(string categoryName = "All")
        {
            var categories = _context.Categories!;
            return (categoryName == "All") ? categories : categories.Where(c => c.Name == categoryName);
        }

        public Animal? GetAnimal(int id) => _context.Animals!.FirstOrDefault(a => a.Id == id);

        public void AddComment(int animalId, string comment)
        {
            var animal = GetAnimal(animalId);
            if (animal != null)
            {
                animal.Comments!.Add(new Comment { Content = comment });
                SaveChanges();
            }
        }

        public Category? GetCategoryById(int id) => _context.Categories!.FirstOrDefault(c => c.Id == id);

        public void DeleteAnimal(int id)
        {
            var animal = GetAnimal(id);
            if (animal != null)
            {
                foreach (var comment in animal.Comments!)
                    _context.Comments!.Remove(comment);

                _context.Animals!.Remove(animal);
            }
            SaveChanges();
        }

        public void SaveChanges() => _context.SaveChanges();

        public IEnumerable<Animal> SearchAnimals(string text) =>
            _context.Animals!.Where(a => a.Name!.ToUpper().Contains(text.ToUpper()));

        public void DeleteCategory(int id)
        {
            var category = GetCategoryById(id);
            if (category != null && category!.Animals!.Count == 0)
            {
                _context.Categories!.Remove(category);
                SaveChanges();
            }
        }

        public void AddCategory(Category newCategory)
        {
            _context.Categories!.Add(newCategory);
            SaveChanges();
        }
    }
}