using Autofac;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using SoftwareRegistration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace POSSystem.UI.Service
{
    public class RegistrationService
    {
        private static string _connString = "";
        private static string _tableName = "";
        private static string _columnName = "";
        private static IRegistration registration;
        private static string _appName;

        static RegistrationService()
        {
            _connString = ConfigurationManager.ConnectionStrings["CloathingPOS"].ConnectionString;
            _tableName = "License";
            _columnName = "SerialKey";
            _appName = StaticContainer.ApplicationName;
            registration = new DatabaseRegistration(_connString, _tableName, _columnName);
        }

        public static void Register(string key, ILog log)
        {
            try
            {
                bool isRegisterSuccess = registration.Register(key);
                if (isRegisterSuccess)
                {
                    StaticContainer.ShowNotification(_appName, "Serial key successfully applied", NotificationType.Success);
                }
                else
                {
                    StaticContainer.ShowNotification(_appName, "Invalid serial key", NotificationType.Warning);
                }
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification(_appName, StaticContainer.ErrorMessage, NotificationType.Error);
                log.Error("OnRegistrationExecute", ex);
            }
        }

        public static bool IsRegistered(MetroWindow window)
        {
            int expiryDays = 0;
            bool isRegistered = registration.IsRegistered(out expiryDays);

            if (!isRegistered)
            {
                window.Show();
                MessageDialogResult result = window.ShowModalMessageExternal(_appName, $"Your software activation is expired. Would you like to reactivate the software", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    RegistrationWindow registrationWindow = StaticContainer.Container.Resolve<RegistrationWindow>();
                    registrationWindow.ShowDialog();
                }
                else
                {
                    window.ShowModalMessageExternal(_appName, $"Application is closing now.", MessageDialogStyle.Affirmative);
                    Application.Current.Shutdown();
                }
            }
            else if (expiryDays <= 15)
            {
                window.Show();
                string msgAppender = (expiryDays == 0) ? "after today midnight" : $"in {expiryDays} days";
                MessageDialogResult result = window.ShowModalMessageExternal(_appName, $"Your software activation will expires {msgAppender}. Would you like to reactivate the software", MessageDialogStyle.AffirmativeAndNegative);
                if (result == MessageDialogResult.Affirmative)
                {
                    RegistrationWindow registrationWindow = StaticContainer.Container.Resolve<RegistrationWindow>();
                    registrationWindow.ShowDialog();
                }
            }

            return isRegistered;
        }
    }
}
