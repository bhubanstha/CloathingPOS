using Autofac;
using ControlzEx.Theming;
using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities.PDF;
using POSSystem.UI.Service;
using POSSystem.UI.Views;
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
            logger = new Logger().GetLogger(typeof(App));
            logger.Info("Starting Application");

            //            AppDomain.CurrentDomain.UnhandledException
            //Application.Current.DispatcherUnhandledException
            //TaskScheduler.UnobservedTaskException


            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            var startup = new Startup();
            var container = startup.BootstrapDependencies();

            StaticContainer.ThisApp = this;
            StaticContainer.Container = container;
            StaticContainer.NotificationManager = new Notifications.Wpf.NotificationManager();

            logger.Info("Bootrap completed");

            logger.Info("Reloading Configuration");
            ReloadConfig();

            //CreateQRCode code = new CreateQRCode();
            //code.CreateLabel();

            var window = container.Resolve<LoginWindow>();
            this.MainWindow = window;
            logger.Info("Showing Login Window");
            //Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            window.Show();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            logger.Error(e.Exception.ToString());
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            logger.Error(e.ToString());
        }

        
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

            logger.Error(e.Exception.Message);
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
                //throw ex;
                // Handle when file is not found in isolated storage:
                // * When the first application session
                // * When file has been deleted
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

