namespace PetShopProj.Models
{
    public class Category
    {
        public Category()
        {
            Animals = new HashSet<Animal>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual ICollection<Animal> Animals { get; set; }
    }
}