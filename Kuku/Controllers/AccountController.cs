using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kuku.ViewModels;
using Kuku.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Kuku.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
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
                string userName = model.Email.Substring(0, model.Email.LastIndexOf('@'));
                User user = new User { Email = model.Email, UserName = userName };
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

        [HttpGet]
        public IActionResult Login()
        {
            string url = Request.Headers["Referer"].ToString();
            Regex regexHome = new Regex(@"\w*Home\w*");
            Regex regexFilter = new Regex(@"\w*filter?\w*");
            MatchCollection matchesHome = regexHome.Matches(url);
            MatchCollection matchesFilter = regexFilter.Matches(url);
            if (matchesHome.Count > 0)
            {
                return View(new LoginViewModel { ReturnUrl = url.Substring(url.LastIndexOf("/Home")) });
            }
            else
            {
                if (matchesFilter.Count > 0)
                {
                    return View(new LoginViewModel { ReturnUrl = url.Substring(url.LastIndexOf("/filter?")) });
                }
                else
                {
                    return View(new LoginViewModel { ReturnUrl = url.Substring(url.LastIndexOf("/")) });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and / or password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // remove authentication cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}