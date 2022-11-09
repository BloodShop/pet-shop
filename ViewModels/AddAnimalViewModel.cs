using PetShopProj.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
    public class AddAnimalViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "The field {0} must be greater than 0.")]
        public double Age { get; set; }
        public IFormFile? Picture { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? AllCategories { get; set; }
    }
}
