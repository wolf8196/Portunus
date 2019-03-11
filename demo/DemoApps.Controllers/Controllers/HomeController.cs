using System.Collections.Generic;
using DemoApps.Controllers.Settings;
using Microsoft.AspNetCore.Mvc;

namespace DemoApps.Controllers
{
    public class HomeController : Controller
    {
        private readonly List<(string dispName, string appName)> availableSSOTargets;

        public HomeController(AvailableSSOTargets availableSSOTargets)
        {
            this.availableSSOTargets = availableSSOTargets.Targets;
        }

        public IActionResult Index()
        {
            return View(availableSSOTargets);
        }
    }
}