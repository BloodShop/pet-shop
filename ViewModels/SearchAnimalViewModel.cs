using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
    public class SearchAnimalViewModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Name cannot exceed 25 characters.")]
        public string? Content { get; set; }
    }
}