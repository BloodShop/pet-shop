using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PetShopProj.Models.HelperModels.NonGeneric
{
    public class CatalogTupleModel : ITuple
    {
        [DisplayName("Item1")]
        public IEnumerable<SelectListItem> CategoriesOptions { get; private set; }

        [DisplayName("Item2")]
        public IEnumerable<Category> SelectedCategory { get; private set; }

        public CatalogTupleModel(
            IEnumerable<SelectListItem> categoriesOptions, 
            IEnumerable<Category> selectedCategory)
        {
            CategoriesOptions = categoriesOptions;
            SelectedCategory = selectedCategory;
        }

        public object? this[int index] => GetType().GetProperties()[index];
        public int Length => GetType().GetProperties().Length - typeof(ITuple).GetMethods().Length;
    }
}