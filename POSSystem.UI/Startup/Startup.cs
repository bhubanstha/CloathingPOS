using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using POSSystem.UI.ViewModel.Service;
using POSSystem.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Startup
{
    public class Startup
    {
        public IContainer BootstrapDependencies()
        {
            var builder = new ContainerBuilder();

            //View Registration
            builder.RegisterType<CreateUserView>().AsSelf();
            builder.RegisterType<InventoryView>().AsSelf();
            

            //Window Registration
            builder.RegisterType<LoginWindow>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();

            //Service Registration
            builder.RegisterType<CacheService>().As<ICacheService>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            
            //View Model Registration
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();

            //Db Context Registration
            builder.RegisterType<POSDataContext>().AsSelf();

            //Framework DI
            builder.RegisterType<DialogCoordinator>().As<IDialogCoordinator>();

            return builder.Build();
        }
    }
}
