using System.Collections.Generic;

namespace Portunus.SSO.Settings
{
    public class SSOSettings
    {
        public SSOSettings()
        {
            Apps = new List<SSOTargetSettings>();
        }

        public List<SSOTargetSettings> Apps { get; set; }
    }
}