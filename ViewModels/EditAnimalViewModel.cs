namespace PetShopProj.ViewModels
{
    public class EditAnimalViewModel : AddAnimalViewModel
    {
        public int Id { get; set; }
        public string? ExistingPhotoPath { get; set; }
    }
}