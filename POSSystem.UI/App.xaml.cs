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
            StaticContainer.DialogCoordinator = window.DialogCoordinator;

            window.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {

        }
    }
}
