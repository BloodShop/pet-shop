using PetShopProj.Models;
using PetShopProj.Models.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
    public class AddAnimalViewModel /*: IValidatableObject*/
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "The field {0} must be between 1 and 120.")]
        public int Age { get; set; }

        [DataType(DataType.Upload)]
        [ValidateFileAttribute(5 * 1024 * 1024)]
        //[MaxFileSize(5 * 1024)]
        //[AllowedExtensions(new string[] { ".jpg", ".png", ".gif" })]
        public IFormFile? Picture { get; set; } 

        [Required]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? AllCategories { get; set; }

        /*public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var photo = ((AddAnimalViewModel)validationContext.ObjectInstance).Picture;
            var extension = Path.GetExtension(photo.FileName);
            var size = photo.Length;

            if (!extension.ToLower().Equals(".jpg") || !extension.ToLower().Equals(".jpg") || !extension.ToLower().Equals(".gif"))
                yield return new ValidationResult("File extension is not valid.");

            if (size > (5 * 1024 * 1024))
                yield return new ValidationResult("File size is bigger than 5MB.");
        }*/
    }
}