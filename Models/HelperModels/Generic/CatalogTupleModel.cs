using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PetShopProj.Models.HelperModels.Generic
{
    public class CatalogTupleModel<IEs, IEc> : ITuple
    {
        [DisplayName("Item1")]
        public IEs CategoriesOptions { get; private set; }

        [DisplayName("Item2")]
        public IEc SelectedCategory { get; private set; }

        public CatalogTupleModel(IEs categoriesOptions, IEc selectedCategory)
        {
            CategoriesOptions = categoriesOptions;
            SelectedCategory = selectedCategory;
        }

        public object? this[int index] => GetType().GetProperties()[index];
        public int Length => GetType().GetProperties().Length - typeof(ITuple).GetMethods().Length;
    }
}