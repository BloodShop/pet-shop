using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetShopProj.Models;
using PetShopProj.Models.HelperModels.NonGeneric;
using PetShopProj.Repositories;
using PetShopProj.ViewModels;
using System.Data;

namespace PetShopProj.Controllers
{
    public class HomeController : Controller
    {
        readonly IRepository _repo;
        readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(IRepository repository, IWebHostEnvironment hostingEnvironment)
        {
            _repo = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index() => View(_repo.GetMostPopularAnimals(2));

        public IActionResult Categories(string category = "All")
        {
            IEnumerable<SelectListItem> categoriesOptions = _repo.GetCategory()
                .Select(c => c.Name).Reverse().Append("All").Reverse().Select(c => new SelectListItem()
                {
                    Text = c,
                    Value = c,
                    Selected = c == category
                });
            /*<IEnumerable<SelectListItem>, IEnumerable<Category>>*/
            return View(new CatalogTupleModel(categoriesOptions, _repo.GetCategory(category).ToList()));
        }


        [HttpPost]
        public IActionResult Search(string text)
        {
            //var tpl0 = (animalsByTxt, title);
            //(IEnumerable<Animal> animalsByTxt, string title) tpl1 = (_repo.SearchAnimals(text), text != null ? text : "");
            //var tpl2 = (animalsByTxt: _repo.SearchAnimals(text), title: text != null ? text : "");

            //var title = text != null ? text : "";
            return View(new SearchTupleModel/*<IEnumerable<Animal>, string>*/(_repo.SearchAnimals(text), text));
        }

        [HttpGet]
        public IActionResult Animal(int id) => View(_repo.GetAnimal(id));

        [HttpPost]
        public IActionResult Animal(int id, string comment)
        {
            if (comment != null && comment != string.Empty)
                _repo.AddComment(id, comment);

            return View(_repo.GetAnimal(id));
        }

        public IActionResult BuyAnimal(int id) => View(_repo.GetAnimal(id));


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddAnimal() => View(new AddAnimalViewModel { AllCategories = _repo.GetCategory() });

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
                    Category = _repo.GetCategoryById(model.CategoryId)
                };

                _repo.AddAnimal(newAnimal);
                return RedirectToAction(nameof(Animal), new { id = newAnimal.Id });
            }

            return View("InvalidImageError", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAnimal(int id)
        {
            var animal = _repo.GetAnimal(id);
            EditAnimalViewModel editAnimalViewModel = new EditAnimalViewModel
            {
                Id = animal!.Id,
                Name = animal.Name,
                Age = animal.Age,
                Description = animal.Description,
                CategoryId = animal.Category!.Id,
                ExistingPhotoPath = animal.PicturePath,
                AllCategories = _repo.GetCategory()
            };
            return View(editAnimalViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAnimal(EditAnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var animal = _repo.GetAnimal(model.Id);
                animal!.Name = model.Name;
                animal.Age = model.Age;
                animal.Description = model.Description;
                animal.Category = _repo.GetCategoryById(model.CategoryId);

                if (model.Picture != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "images", "animalsImages", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    animal.PicturePath = UploadImage(model);
                }

                _repo.SaveChanges();
                return RedirectToAction(nameof(Animal), new { id = animal.Id });
            }

            return View("InvalidImageError", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }


        string? UploadImage(AddAnimalViewModel model)
        {
            string? uniqueFileName = null;
            if (ModelState.IsValid/*model.Picture != null && model.Picture.Length < 1e6*/)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "animalsImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Picture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    model.Picture.CopyTo(fileStream);
            }

            return uniqueFileName;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAnimal(int id)
        {
            var animal = _repo.GetAnimal(id);
            if (animal != null && animal.PicturePath != null)
            {
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "animalsImages", animal.PicturePath);
                System.IO.File.Delete(filePath);
            }
            _repo.DeleteAnimal(id);
            return RedirectToAction(nameof(Categories));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageCategories() => View(_repo.GetCategory());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ManageCategories(int id)
        {
            var category = _repo.GetCategoryById(id)!;

            if (category.Animals!.Count != 0)
                return View("RemoveCategoryError");

            _repo.DeleteCategory(id);
            return View(_repo.GetCategory());
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category { Name = model.Name };

                _repo.AddCategory(newCategory);
                return RedirectToAction(nameof(ManageCategories));
            }
            return View();
        }
    }
}