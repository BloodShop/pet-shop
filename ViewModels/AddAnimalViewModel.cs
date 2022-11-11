﻿using PetShopProj.Models;
using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
    public class AddAnimalViewModel
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters.")]
        public string? Name { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "The field {0} must be between 1 and 120.")]
        public int Age { get; set; }

        public IFormFile? Picture { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? AllCategories { get; set; }
    }
}