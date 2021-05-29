using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Service
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
