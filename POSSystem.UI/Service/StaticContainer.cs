using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.Model;
using POS.Model.ViewModel;
using POSSystem.UI.ViewModel;
using POSSystem.UI.Views.Dialog;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Media.Imaging;

namespace POSSystem.UI.Service
{
    public class SplashScreenMessage: ViewModelBase
    {
        private  string _msg="Registering container and services";

        public  string Message
        {
            get { return _msg; }
            set { 
                _msg = value;
                OnPropertyChanged();
            }
        }

    }
    public static class StaticContainer
    {
        public static int AppDeployedYear { get; private set; }
        public static string ApplicationName { get; set; }
        //public static BitmapImage AppScreenshot { get; set; }

        public static App ThisApp { get; set; }
        public static IContainer Container { get; set; }

        public static Flyout SettingFlyout { get; set; }
        public static Flyout AddCategoryFlyout { get; set; }
        public static Flyout AddBrandFlyout { get; set; }
        public static Flyout AddBranchFlyout { get; set; }
        public static Flyout NoSearchResultFlyout { get; set; }

        public static IDialogCoordinator DialogCoordinator { get; set; }

        public static MetroDialogSettings DialogSettings{get ; private set;}
        public static CultureInfo CultureInfo { get; set; }
        public static NotificationManager NotificationManager { get; set; }
        public static string SettingFile { get; set; }
        public static bool IsPasswordChanged { get; set; }
        public static ShopVM Shop { get; set; }
        public static string ErrorMessage { get; private set; } = "Something went wrong. Please contact system admin for support.";

        public static HamburgerMenu UIHamburgerMenuControl { get; set; }
        public static CustomDialog InventoryUpdateDialog { get; set; }
        public static CustomDialog ChangeUserPasswordDialog { get; set; }
        public static CustomDialog AddNewCustomerDialog { get; set; }

        public static Int64 ActiveBranchId { get; set; }

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
            AppDeployedYear = ConfigurationReader.GetConfiguration <int>(AppSettingKey.AppDeployedYear);
            string culture = ConfigurationReader.GetConfiguration<string>(AppSettingKey.CurrencyCulture);
            CultureInfo = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures).Where(c => c.DisplayName == culture).FirstOrDefault();
        }

        public static void ShowNotification(string title, string message, NotificationType type)
        {
            NotificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            });
        }

        public static void UpdateShop(Shop s)
        {
            Shop.Id = s.Id;
            Shop.LogoPath = s.LogoPath;
            Shop.Name = s.Name;
            Shop.PANNumber = s.PANNumber;
            Shop.PdfPassword = s.PdfPassword;
            Shop.PrintInvoice = s.PrintInvoice;
        }

    }
}
