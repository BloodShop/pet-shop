using Microsoft.AspNetCore.Mvc;

namespace PetShopProj.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
