using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace AA2Install
{
    static class Configuration
    {
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                Trace.WriteLine(key + " : " + result);
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Trace.WriteLine("Error reading app settings");
                return null;
            }
        }
        public static bool WriteSetting(string key, string value)
        {
            try
            {
                Trace.WriteLine(key + " : " + value);
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                return true;
            }
            catch (ConfigurationErrorsException)
            {
                Trace.WriteLine("Error writing app settings");
                return false;
            }
        }

        #region Quick Access
        public static bool isPPRAW
        {
            get
            {
                return bool.Parse(Configuration.ReadSetting("PPRAW") ?? "False");
            }
        }
        #endregion
    }
}
