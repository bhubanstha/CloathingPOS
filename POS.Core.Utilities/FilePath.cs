using System;
using System.IO;

namespace POS.Core.Utilities
{
    public static class FilePath
    {

        public static string GetLogoFullPath(string logoName)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "CompanyLogo", logoName);
            return logoPath;
        }

        public static string GetProfileImageFullPath(string profileImageName)
        {
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "Profile", profileImageName);
            return logoPath;
        }
    }
}
