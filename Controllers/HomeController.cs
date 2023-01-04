using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using PetShopProj.Hubs;
using PetShopProj.Models;
using PetShopProj.Models.HelperModels.NonGeneric;
using PetShopProj.Repositories;
using PetShopProj.ViewModels;
using System.Data;
using System.Diagnostics;

namespace PetShopProj.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger _logger;
        readonly IRepository _repo;
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly IMemoryCache _memoryCache;
        readonly IHubContext<CallCenterHub, ICallCenterHub> _hubContext;
        const string ANIMAL_KEY = "animale"; // MemoryCache
        const string VISIT_COUNT_KEY = "Visit_Count"; // Session 

        public HomeController(IRepository repository,
            IWebHostEnvironment hostingEnvironment,
            ILogger<HomeController> logger,
            IMemoryCache memoryCache,
            IHubContext<CallCenterHub, ICallCenterHub> hubContext)
        {
            _logger = logger;
            _repo = repository;
            _memoryCache = memoryCache;
            _hubContext = hubContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            int? visitorCount = HttpContext.Session.GetInt32(VISIT_COUNT_KEY);
            if (visitorCount.HasValue)
                visitorCount++;
            else
                visitorCount = 1;

            HttpContext.Session.SetInt32(VISIT_COUNT_KEY, visitorCount.Value);
            _logger.LogInformation($"(session) Number of Home visits: {visitorCount}");

            return View(_repo.GetMostPopularAnimals(2));
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        public IActionResult AjaxRequest() => View(); // Example of ajax request

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
        public IActionResult Search(string text = "")
        {
            //var tpl0 = (animalsByTxt, title);
            //(IEnumerable<Animal> animalsByTxt, string title) tpl1 = (_repo.SearchAnimals(text), text != null ? text : "");
            //var tpl2 = (animalsByTxt: _repo.SearchAnimals(text), title: text != null ? text : "");

            //var title = text != null ? text : "";
            return View(new SearchTupleModel/*<IEnumerable<Animal>, string>*/(_repo.SearchAnimals(text), text));
        }

        [HttpGet]
        public IActionResult Animal(int id)
        {
            if (id <= 0)
            {
                _logger.LogError($"{id} not valid id attempt");
                this.HttpContext.Response.StatusCode = 418; // I'm a teapot
                return StatusCode(StatusCodes.Status418ImATeapot);
            }

            Animal animal;
            var secretKeyById = $"{ANIMAL_KEY}{id}";

            if (!_memoryCache.TryGetValue<Animal>(secretKeyById, out animal))
            {
                animal = _repo.GetAnimal(id);
                MemoryCacheEntryOptions options = new();
                options.SetPriority(CacheItemPriority.Low);
                options.SetSlidingExpiration(new TimeSpan(1000000));
                _memoryCache.Set(secretKeyById, animal/*, options*/);
            }

            return View(animal);
        }

        [HttpPost]
        public IActionResult Animal(int id, string comment)
        {
            if (comment != null && comment != string.Empty)
            {
                _repo.AddComment(id, comment);
                _logger.LogInformation("!! Attempted to add an empty comment");
            }

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
                Animal newAnimal = new()
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

            _logger.LogError("!! Attempted to add an invalid image");
            return View("InvalidImageError", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditAnimal(int id)
        {
            var animal = _repo.GetAnimal(id);
            EditAnimalViewModel editAnimalViewModel = new()
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

            _logger.LogError("!! Attempted to add an invalid image");
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
            {
                _logger.LogError("!! Attempted to remove category with animals.");
                return View("RemoveCategoryError");
            }

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
                _logger.LogInformation("A category has been added.");
                return RedirectToAction(nameof(ManageCategories));
            }
            return View();
        }
    }
}