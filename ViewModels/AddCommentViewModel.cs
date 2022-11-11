using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
	public class AddCommentViewModel
	{
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string Content { get; set; } = null!;
    }
}