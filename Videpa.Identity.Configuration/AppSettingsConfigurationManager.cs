using System;
using System.Configuration;
using Videpa.Identity.Logic.Ports;

namespace Videpa.Identity.Configuration
{
    public class AppSettingsConfigurationManager : IConfigurationManager
    {
        private static bool GetBooleanAppSetting(string key)
        {
            return Convert.ToBoolean(GetStringAppSetting(key));
        }

        private static string GetStringAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public bool FakeIdentity { get { return GetBooleanAppSetting("FakeIdentity"); } }
        public bool ShowStackTraceInErrorResponse { get { return GetBooleanAppSetting("ShowStackTraceInErrorResponse"); } }
        public string JwtHeaderKey { get { return GetStringAppSetting("JwtHeaderKey"); } }
    }
}
