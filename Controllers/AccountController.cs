using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetShopProj.ViewModels;

namespace PetShopProj.Controllers
{
    public class AccountController : Controller
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly RoleManager<IdentityRole> _roleManager;
        readonly ILogger<HomeController> _logger;

        public AccountController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation($"An account has logged out !!");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login() => View();
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"{model.UserName} has logged in !!");
                    return RedirectToAction("Index", "Home");
                }

                _logger.LogInformation($"!! Invalid Login Attempt username: {model.UserName}, pass:{model.Password}");
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register() => View();
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation($"New registration - username: {model.UserName}, pass: {model.Password}");
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogError("!!" + error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var role = await _roleManager.FindByNameAsync("Admin");
            var model = new List<ManageUsersViewModel>();

            foreach (var user in _userManager.Users.Where(u => u.UserName != User.Identity!.Name).ToList())
            {
                var userRoleViewModel = new ManageUsersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userRoleViewModel.IsSelected = true;
                else
                    userRoleViewModel.IsSelected = false;
                
                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers(List<ManageUsersViewModel> model)
        {
            var role = await _roleManager.FindByNameAsync("Admin");

            foreach (var item in model)
            {
                var user = await _userManager.FindByIdAsync(item.UserId);

                if (item.IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                    await _userManager.AddToRoleAsync(user, role.Name);
                else if (!item.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return View(model);
        }
    }
}