using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kuku.ViewModels;
using Kuku.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<KukuUser> _userManager;
        private readonly SignInManager<KukuUser> _signInManager;

        public AccountController(UserManager<KukuUser> userManager, SignInManager<KukuUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                KukuUser user = new KukuUser { UserLogin = model.UserLogin, UserName = model.Name, Surname = model.Surname, Patronymic = model.Patronymic };
                // add user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // installation of cookies
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
    }
}