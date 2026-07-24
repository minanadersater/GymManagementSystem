using GymManagementSystem.BLL.ViewModels.ApplicationViewModels;
using GymManagementSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;     // ✅ اتصلحت
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);

            if (user is null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password");
                return View(model);
            }

            var Result = await signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);

            if (Result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Result.IsLockedOut)
                    ModelState.AddModelError(string.Empty, "This account has been locked out.");
                else if (Result.IsNotAllowed)
                    ModelState.AddModelError(string.Empty, "This account is not allowed to sign in.");
                else
                    ModelState.AddModelError(string.Empty, "Invalid Email or Password");

                return View(model);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}