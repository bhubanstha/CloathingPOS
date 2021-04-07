using Autofac;
using MahApps.Metro.Controls;
using POS.Data;
using POS.Data.Repository;
using POS.Model;
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

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<LoginWindow>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<POSDataContext>().AsSelf();

            return builder.Build();
        }
    }
}
