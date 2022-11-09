using System.ComponentModel.DataAnnotations;

namespace PetShopProj.ViewModels
{
	public class AddCategoryViewModel
	{
		[Required]
		public string? Name { get; set; }
	}
}