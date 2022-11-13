using System.ComponentModel.DataAnnotations;

namespace PetShopProj.Models.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidateFileAttribute : ValidationAttribute
    {
        double _maxContent = 1e2; //1 MB
        string[] sAllowedExt = new string[] { ".jpg", ".gif", ".png" };

        public ValidateFileAttribute(long maxContent) => _maxContent = maxContent;

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;

            if (file == null)
            {
                ErrorMessage = "Please upload any photo";
                return false;
            }
            
            if (!sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(" / ", sAllowedExt);
                return false;
            }

            if (file.Length > _maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (_maxContent / 1024).ToString() + "MB";
                return false;
            }

            return true;
        }
    }
}