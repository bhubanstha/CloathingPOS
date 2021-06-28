using System;
using System.Configuration;

namespace POSSystem.WPF.UI.Service
{
    public class ConfigurationReader
    {
        public static T GetConfiguration<T>(AppSettingKey key)
        {
            string val = ConfigurationManager.AppSettings[key.ToString()];

            return (T)Convert.ChangeType(val, typeof(T));
        }
    }

    public enum AppSettingKey
    {
        CurrencyCulture,
        AppName,
        PdfPassword
    }
}
