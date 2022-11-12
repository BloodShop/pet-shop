using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PetShopProj.Models.HelperModels.Generic
{
    public class SearchTupleModel<IE, s> : ITuple
    {
        [DisplayName("Item1")]
        public IE AnimalsByTitle { get; private set; }

        [DisplayName("Item2")]
        public s Title { get; }

        public SearchTupleModel(IE animalsByTxt, s title)
        {
            AnimalsByTitle = animalsByTxt;
            Title = title;
        }

        public object? this[int index] => GetType().GetProperties()[index];
        public int Length => GetType().GetProperties().Length - typeof(ITuple).GetMethods().Length;
    }
}