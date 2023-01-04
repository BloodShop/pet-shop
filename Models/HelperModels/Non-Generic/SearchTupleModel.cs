using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PetShopProj.Models.HelperModels.NonGeneric
{
    public class SearchTupleModel : ITuple
    {
        [DisplayName("Item1")]
        public IEnumerable<Animal> AnimalsByTitle { get; private set; }

        [DisplayName("Item2")]
        public string Title { get; private set; } = null!;

        public SearchTupleModel(IEnumerable<Animal> animalsByTxt, string title)
        {
            AnimalsByTitle = animalsByTxt;
            Title = title;
        }

        public object? this[int index] => GetType().GetProperties()[index];
        public int Length => GetType().GetProperties().Length - typeof(ITuple).GetMethods().Length;
    }
}