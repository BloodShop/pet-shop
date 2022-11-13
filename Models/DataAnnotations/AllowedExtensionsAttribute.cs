using System.ComponentModel.DataAnnotations;

namespace PetShopProj.Models.DataAnnotations
{
    public class AllowedExtensionsAttribute : ValidationAttribute
    {
        readonly string[] _extensions;
        public AllowedExtensionsAttribute(params string[] extensions) => _extensions = extensions;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName);
                if (!_extensions.Contains(extension.ToLower()))
                    return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage() => $"This photo extension is not allowed!";
    }
}