using System.Threading.Tasks;
using DemoApps.Controllers.Settings;
using DemoApps.Infrastructure;
using DemoApps.Shared.Identity;
using DemoApps.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApps.Controllers
{
    public class SSOController : Controller
    {
        private readonly UserStore userStore;
        private readonly UserManager userManager;
        private readonly PortunusSSOService ssoService;
        private readonly SSOSettings settings;

        public SSOController(
            UserStore userStore,
            UserManager userManager,
            PortunusSSOService ssoService,
            SSOSettings settings)
        {
            this.userStore = userStore;
            this.userManager = userManager;
            this.ssoService = ssoService;
            this.settings = settings;
        }

        [Authorize]
        public async Task<IActionResult> To(string id)
        {
            var app = id;
            var user = userStore.FindByUserName(User.Identity.Name);

            var destinationUrl = await ssoService.GetTargetAuthAddressAsync(
                app,
                new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                });
            
            return Redirect(destinationUrl);
        }

        public async Task<IActionResult> Login(string id)
        {
            var token = id;
            var payload = await ssoService.VerifyToken<UserViewModel>(settings.CurrentAppName, token);

            if (payload == null)
            {
                return RedirectToAction("Error");
            }

            var user = userStore.FindByUserName(payload.UserName);

            if (user != null)
            {
                await userManager.SignOutAsync(HttpContext);
                await userManager.SignInAsync(HttpContext, user);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Error");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}