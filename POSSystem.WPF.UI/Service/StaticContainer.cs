using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf.Core;
using POS.Core.Model;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;

namespace POSSystem.WPF.UI.Service
{
    public static class StaticContainer
    {
        public static string ApplicationName { get; set; }
        public static BitmapImage AppScreenshot { get; set; }

        public static App ThisApp { get; set; }
        public static IContainer Container { get; set; }

        public static Flyout SettingFlyout { get; set; }
        public static Flyout AddCategoryFlyout { get; set; }
        public static Flyout AddBrandFlyout { get; set; }
        public static Flyout NoSearchResultFlyout { get; set; }

        public static IDialogCoordinator DialogCoordinator { get; set; }

        public static MetroDialogSettings DialogSettings{get ; private set;}
        public static CultureInfo CultureInfo { get; set; }
        public static NotificationManager NotificationManager { get; set; }
        public static string SettingFile { get; set; }
        public static bool IsPasswordChanged { get; set; }
        public static Shop Shop { get; set; }
        public static string ErrorMessage { get; private set; } = "Something went wrong. Please contact system admin for support.";

        public static HamburgerMenu UIHamburgerMenuControl { get; set; }
        public static CustomDialog InventoryUpdateDialog { get; set; }
        public static CustomDialog ChangeUserPasswordDialog { get; set; }
        static StaticContainer()
        {
            DialogSettings = new MetroDialogSettings
            {
                AnimateShow = true,
                AnimateHide = true,
                DefaultButtonFocus = MessageDialogResult.Affirmative,
                AffirmativeButtonText = "OK",
                OwnerCanCloseWithDialog = true,
                ColorScheme = MetroDialogColorScheme.Theme
            };

            SettingFile = "AppConfiguration.txt";// Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppConfiguration.txt");
            ApplicationName = ConfigurationReader.GetConfiguration <string>(AppSettingKey.AppName);
            string culture = ConfigurationReader.GetConfiguration<string>(AppSettingKey.CurrencyCulture);
            CultureInfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures).Where(c => c.DisplayName == culture).FirstOrDefault();
        }

        public static void ShowNotification(string title, string message, NotificationType type)
        {
            NotificationManager.ShowAsync(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            });
        }

    }
}
