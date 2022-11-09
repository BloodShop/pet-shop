using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetShopProj.Models;
using PetShopProj.Repositories;
using PetShopProj.ViewModels;
using System.Data;

namespace PetShopProj.Controllers
{
    public class HomeController : Controller
    {
        private IRepository repository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            this.repository = repository;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(repository.GetMostPopularAnimals(2));
        }

        public IActionResult Categories(string category = "All")
        {
            IEnumerable<SelectListItem> categoriesOptions = repository!.GetCategory()
                .Select(c => c.Name).Reverse().Append("All").Reverse().Select(c => new SelectListItem
                {
                    Text = c,
                    Value = c,
                    Selected = c == category
                });

            return View(new Tuple<IEnumerable<SelectListItem>, IEnumerable<Category>>(categoriesOptions, repository.GetCategory(category)));
        }

        [HttpPost]
        public IActionResult Search(string text)
        {
            return View(new Tuple<IEnumerable<Animal>, string>(repository.SearchAnimals(text), text));
        }

        [HttpGet]
        public IActionResult Animal(int id)
        {
            return View(repository.GetAnimal(id));
        }

        [HttpPost]
        public IActionResult Animal(int id, string comment)
        {
            repository.AddComment(id, comment);
            return View(repository.GetAnimal(id));
        }

        public IActionResult BuyAnimal(int id)
        {
            return View(repository.GetAnimal(id));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAnimal()
        {
            return View(new AddAnimalViewModel { AllCategories = repository.GetCategory() });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAnimal(AddAnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Animal newAnimal = new Animal
                {
                    Name = model.Name,
                    Age = model.Age,
                    Description = model.Description,
                    PicturePath = UploadImage(model),
                    Category = repository.GetCategoryById(model.CategoryId)
                };

                repository.AddAnimal(newAnimal);
                return RedirectToAction(nameof(Animal), new { id = newAnimal.Id });
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAnimal(int id)
        {
            var animal = repository.GetAnimal(id);
            EditAnimalViewModel editAnimalViewModel = new EditAnimalViewModel
            {
                Id = animal!.Id,
                Name = animal.Name,
                Age = animal.Age,
                Description = animal.Description,
                CategoryId = animal.Category!.Id,
                ExistingPhotoPath = animal.PicturePath,
                AllCategories = repository.GetCategory()
            };
            return View(editAnimalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAnimal(EditAnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var animal = repository.GetAnimal(model.Id);
                animal!.Name = model.Name;
                animal.Age = model.Age;
                animal.Description = model.Description;
                animal.Category = repository.GetCategoryById(model.CategoryId);

                if (model.Picture != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", "animalsImages", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    animal.PicturePath = UploadImage(model);
                }

                repository.Update();
                return RedirectToAction(nameof(Animal), new { id = animal.Id });
            }

            return View();
        }

        private string? UploadImage(AddAnimalViewModel model)
        {
            string? uniqueFileName = null;
            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images", "animalsImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = repository.GetAnimal(id);
            if (animal != null && animal.PicturePath != null)
            {
                var filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", "animalsImages", animal.PicturePath);
                System.IO.File.Delete(filePath);
            }
            repository.DeleteAnimal(id);
            return RedirectToAction(nameof(Categories));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageCategories()
        {
            return View(repository.GetCategory());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageCategories(int id)
        {
            Category category = repository.GetCategoryById(id)!;

            if (category.Animals!.Count != 0)
            {
                return View("RemoveCategoryError");
            }
            repository.DeleteCategory(id);
            return View(repository.GetCategory());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category newCategory = new Category { Name = model.Name };

                repository.AddCategory(newCategory);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View();
        }
    }
}
