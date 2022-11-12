using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
    public class SearchAnimalViewModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "{0} cannot exceed 25 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Content { get; set; } = null!;
    }
}