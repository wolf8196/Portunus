using Portunus.TokenProvider.Settings;

namespace Portunus.SSO.Settings
{
    public class SSOTargetSettings
    {
        public string AppName { get; set; }

        public string AuthenticationUrlTemplate { get; set; }

        public int ExpireInSeconds { get; set; }

        public TokenProviderSettings TokenProviderSettings { get; set; }
    }
}