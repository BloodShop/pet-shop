using System.ComponentModel.DataAnnotations;

namespace PetShopProj.Models.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidateFileAttribute : ValidationAttribute
    {
        double _maxContent = 1 * 1024 * 1024; //1 MB
        string[] _sAllowedExt = new string[] { ".jpg", ".gif", ".png", ".jpeg", ".jpeg2000" };

        public ValidateFileAttribute(long maxContent) => _maxContent = maxContent;
        public ValidateFileAttribute(long maxContent, params string[] extentions)
        {
            _maxContent = maxContent;
            _sAllowedExt = extentions;
        }

        public override bool IsValid(object? value)
        {
            var file = value as IFormFile;

            if (file == null)
                return true;

            if (!_sAllowedExt.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
            {
                ErrorMessage = "Please upload Your Photo of type: " + string.Join(" / ", _sAllowedExt);
                return false;
            }

            if (file.Length > _maxContent)
            {
                ErrorMessage = "Your Photo is too large, maximum allowed size is : " + (_maxContent / 1024).ToString() + "MB";
                return false;
            }

            return true;
        }

        public static bool IsFileValid(IFormFile file)
        {
            using (var reader = new BinaryReader(file.OpenReadStream()))
            {
                var signatures = _fileSignatures.Values.SelectMany(x => x).ToList();  // flatten all signatures to single list
                var headerBytes = reader.ReadBytes(_fileSignatures.Max(m => m.Value.Max(n => n.Length)));
                bool result = signatures.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                return result;
            }
        }

        static readonly Dictionary<string, List<byte[]>> _fileSignatures = new()
        {
            { ".gif", new List<byte[]> { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".png", new List<byte[]> { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } } },
            { ".jpeg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                }
            },
            { ".jpeg2000", new List<byte[]> { new byte[] { 0x00, 0x00, 0x00, 0x0C, 0x6A, 0x50, 0x20, 0x20, 0x0D, 0x0A, 0x87, 0x0A } } },

            { ".jpg", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                    new byte[] { 0xFF, 0xD8, 0xFF, 0xDB },
                }
            },
            { ".zip", new List<byte[]> //also docx, xlsx, pptx, ...
                {
                    new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                    new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                    new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                    new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                    new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                    new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                }
            },

            { ".pdf", new List<byte[]> { new byte[] { 0x25, 0x50, 0x44, 0x46 } } },
            { ".z", new List<byte[]>
                {
                    new byte[] { 0x1F, 0x9D },
                    new byte[] { 0x1F, 0xA0 }
                }
            },
            { ".tar", new List<byte[]>
                {
                    new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x00, 0x30 , 0x30 },
                    new byte[] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20 , 0x00 },
                }
            },
            { ".tar.z", new List<byte[]>
                {
                    new byte[] { 0x1F, 0x9D },
                    new byte[] { 0x1F, 0xA0 }
                }
            },
            { ".tif", new List<byte[]>
                {
                    new byte[] { 0x49, 0x49, 0x2A, 0x00 },
                    new byte[] { 0x4D, 0x4D, 0x00, 0x2A }
                }
            },
            { ".tiff", new List<byte[]>
                {
                    new byte[] { 0x49, 0x49, 0x2A, 0x00 },
                    new byte[] { 0x4D, 0x4D, 0x00, 0x2A }
                }
            },
            { ".rar", new List<byte[]>
                {
                    new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07 , 0x00 },
                    new byte[] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07 , 0x01, 0x00 },
                }
            },
            { ".7z", new List<byte[]>
                {
                    new byte[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27 , 0x1C },
                }
            },
            { ".txt", new List<byte[]>
                {
                    new byte[] { 0xEF, 0xBB , 0xBF },
                    new byte[] { 0xFF, 0xFE},
                    new byte[] { 0xFE, 0xFF },
                    new byte[] { 0x00, 0x00, 0xFE, 0xFF },
                }
            },
            { ".mp3", new List<byte[]>
                {
                    new byte[] { 0xFF, 0xFB },
                    new byte[] { 0xFF, 0xF3},
                    new byte[] { 0xFF, 0xF2},
                    new byte[] { 0x49, 0x44, 0x43},
                }
            },
        };
    }
}