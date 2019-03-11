using System.Threading.Tasks;
using DemoApps.Infrastructure;
using DemoApps.Shared.Identity;
using DemoApps.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApps.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserStore userStore;
        private readonly UserManager userManager;
        private readonly PortunusSSOService ssoService;

        public AccountController(UserStore userStore, UserManager userManager, PortunusSSOService ssoService)
        {
            this.userStore = userStore;
            this.userManager = userManager;
            this.ssoService = ssoService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = userStore.FindByUserName(model.Login);

            if (user.Password != model.Password && user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or password is invalid");
                return View(model);
            }

            await userManager.SignInAsync(HttpContext, user);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var user = userStore.FindByUserName(User.Identity.Name);

            return View(new UserViewModel
            {
                UserName = user.UserName,
                Id = user.Id
            });
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await userManager.SignOutAsync(HttpContext);
            return RedirectToActionPermanent("Index", "Home");
        }
    }
}