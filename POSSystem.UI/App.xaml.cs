using Autofac;
using ControlzEx.Theming;
using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities.PDF;
using POSSystem.UI.Service;
using POSSystem.UI.Views;
using SoftwareRegistration;
using System;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;

namespace POSSystem.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ILog logger;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var startup = new Startup();
            var container = startup.BootstrapDependencies();

            logger = container.Resolve<ILogger>().GetLogger(typeof(App));
            logger.Info("Starting Application");

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            StaticContainer.ThisApp = this;
            StaticContainer.Container = container;
            StaticContainer.NotificationManager = new NotificationManager();


            logger.Info("Reloading Configuration");
            ReloadConfig();


            var window = new PrintTestWindow();// container.Resolve<LoginWindow>();
            this.MainWindow = window;

            logger.Info("Checking Software Registration");
            bool isRegistered = RegistrationService.IsRegistered(window);

            if (isRegistered)
            {
                logger.Info("Showing Login Window");
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                window.Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            logger.Fatal("UnobservedTaskException", e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            logger.Fatal("UnhandledException", e.ExceptionObject as Exception);
        }

        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Fatal("DispatcherUnhandledException", e.Exception);
            StaticContainer.NotificationManager.Show(new NotificationContent
            {
                Message = e.Exception.Message,
                Title = "Error",
                Type = NotificationType.Error
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Persist application-scope property to isolated storage
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(StaticContainer.SettingFile, FileMode.Create, storage))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // Persist each application-scope property individually
                foreach (string key in this.Properties.Keys)
                {
                    writer.WriteLine("{0}:{1}", key, this.Properties[key]);
                }
            }
        }

        void ReloadConfig()
        {
            // Restore application-scope property from isolated storage
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForDomain();
            try
            {
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(StaticContainer.SettingFile, FileMode.Open, storage))
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Restore each application-scope property individually
                    while (!reader.EndOfStream)
                    {
                        string[] keyValue = reader.ReadLine().Split(new char[] { ':' });
                        if (keyValue.Length > 1)
                        {
                            this.Properties[keyValue[0]] = keyValue[1];
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                logger.Error("ReloadConfig", ex);
            }
            finally
            {
                string baseColor = this.Properties["BaseColour"] == null ? "Light" : this.Properties["BaseColour"].ToString();
                string colorScheme = this.Properties["ColorScheme"] == null ? "Blue" : this.Properties["ColorScheme"].ToString();

                ThemeManager.Current.ChangeThemeBaseColor(this, baseColor);
                ThemeManager.Current.ChangeThemeColorScheme(this, colorScheme);
            }
        }
    }
}

