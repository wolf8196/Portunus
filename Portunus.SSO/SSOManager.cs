using System.Collections.Generic;
using System.Dynamic;
using Microsoft.Extensions.Options;
using Portunus.SSO.Exceptions;
using Portunus.SSO.Interfaces;
using Portunus.SSO.Settings;
using Portunus.TokenProvider.Interfaces;
using Portunus.Utils;

namespace Portunus.SSO
{
    public class SSOManager : ISSOManager
    {
        private readonly SSOSettings settings;
        private readonly Dictionary<string, SSOService> ssoServices;

        public SSOManager(ITokenProviderFactory factory, IOptions<SSOSettings> options)
        {
            factory.ThrowIfNull(nameof(factory));
            settings = options.ThrowIfNull(nameof(options)).Value;
            settings.ThrowIfNull(nameof(settings));

            ssoServices = new Dictionary<string, SSOService>();

            foreach (var app in settings.Apps)
            {
                var ssoService = new SSOService(factory, app);
                ssoServices.Add(app.AppName.ToLower(), ssoService);
            }
        }

        public string IssueToken(string app, ExpandoObject payload)
        {
            app.ThrowIfNullOrEmpty(nameof(app));
            payload.ThrowIfNull(nameof(payload));

            if (!ssoServices.TryGetValue(app.ToLower(), out SSOService ssoService))
            {
                throw new AppNotRegisteredException(app);
            }

            return ssoService.IssueToken(payload);
        }

        public bool TryVerifyToken(string app, string token, out ExpandoObject payload)
        {
            app.ThrowIfNullOrEmpty(nameof(app));
            token.ThrowIfNullOrEmpty(nameof(token));

            if (!ssoServices.TryGetValue(app.ToLower(), out SSOService ssoService))
            {
                throw new AppNotRegisteredException(app);
            }

            return ssoService.TryVerifyToken(token, out payload);
        }
    }
}