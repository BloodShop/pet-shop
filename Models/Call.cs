using System.ComponentModel.DataAnnotations;

namespace PetShopProj.Models
{
    public class Call
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Problem { get; set; } = null!;
        public DateTime CallTime { get; set; } = DateTime.UtcNow;
        public bool Answered { get; set; } = false;
        public DateTime AnswerTime { get; set; } = DateTime.MinValue;
    }
}