using System;

namespace Portunus.SSO.Exceptions
{
    public class AppNotRegisteredException : Exception
    {
        public AppNotRegisteredException(string appName)
            : base("Requested application was not registered")
        {
            AppName = appName;
        }

        public string AppName { get; set; }

        public override string Message
        {
            get
            {
                return $"{base.Message}.{Environment.NewLine}Application name: {AppName}";
            }
        }
    }
}