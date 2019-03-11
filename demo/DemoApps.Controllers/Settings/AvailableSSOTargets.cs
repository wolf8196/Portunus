using System.Collections.Generic;

namespace DemoApps.Controllers.Settings
{
    public class AvailableSSOTargets
    {
        public List<(string dispName, string appName)> Targets { get; set; }
    }
}