namespace PetShopProj.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public virtual Animal? Animal { get; set; }
    }
}