using System;
using System.IO;

namespace POS.Utilities
{
    public static class FilePath
    {

        public static string GetLogoFullPath(string logoName)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "CompanyLogo", logoName);
            return logoPath;
        }
    }
}
