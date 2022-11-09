namespace PetShopProj.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Age { get; set; }
        public string? PicturePath { get; set; }
        public string? Description { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}