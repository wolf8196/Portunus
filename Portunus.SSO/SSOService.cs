using System;
using System.Dynamic;
using Portunus.SSO.Settings;
using Portunus.TokenProvider.Interfaces;
using Portunus.TokenProvider.Models;
using Portunus.Utils;

namespace Portunus.SSO
{
    internal class SSOService
    {
        private readonly SSOTargetSettings settings;
        private readonly ITokenProvider tokenProvider;

        internal SSOService(ITokenProviderFactory factory, SSOTargetSettings targetSettings)
        {
            settings = targetSettings.ThrowIfNull(nameof(settings));

            settings.AppName.ThrowIfNullOrEmpty(nameof(settings.AppName));
            settings.AuthenticationUrlTemplate.ThrowIfNullOrEmpty(nameof(settings.AuthenticationUrlTemplate));
            settings.TokenProviderSettings.ThrowIfNull(nameof(settings.TokenProviderSettings));

            tokenProvider = factory.Create(settings.TokenProviderSettings);
        }

        public string IssueToken(ExpandoObject payload)
        {
            var tokenObj = new AccessToken
            {
                Payload = payload,
                ExpireOn = DateTime.UtcNow.AddSeconds(settings.ExpireInSeconds)
            };

            var token = tokenProvider.Issue(tokenObj);

            return string.Format(settings.AuthenticationUrlTemplate, token);
        }

        public bool TryVerifyToken(string s, out ExpandoObject payload)
        {
            payload = null;

            if (!tokenProvider.TryVerify(s, out AccessToken token))
            {
                return false;
            }

            if (token.ExpireOn < DateTime.UtcNow)
            {
                return false;
            }

            payload = token.Payload;

            return true;
        }
    }
}