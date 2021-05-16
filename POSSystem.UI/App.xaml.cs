using POSSystem.UI.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using POSSystem.UI.Startup;
using Autofac;
using POSSystem.UI.Service;
using ControlzEx.Theming;
using System.IO.IsolatedStorage;
using System.IO;

namespace POSSystem.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var startup = new Startup.Startup();
            var container = startup.BootstrapDependencies();

            var window = container.Resolve<MainWindow>();
            this.MainWindow = window;


            StaticContainer.ThisApp = this;
            StaticContainer.Container = container;
            StaticContainer.SettingFlyout = window.SettingsFlyout;// container.Resolve<SettingView>();
            StaticContainer.AddCategoryFlyout = window.CategoryFlyout;
            StaticContainer.DialogCoordinator = window.DialogCoordinator;
            ApplyThemeConfig();
            window.Show();


        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

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

        void ApplyThemeConfig()
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
                        this.Properties[keyValue[0]] = keyValue[1];
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
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

