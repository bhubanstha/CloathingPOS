using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSSystem.UI.Views.Dialog
{
    /// <summary>
    /// Interaction logic for AdminChangePassword.xaml
    /// </summary>
    public partial class AdminChangePassword : CustomDialog
    {
        public AdminChangePassword()
        {
            InitializeComponent();
            this.DataContext = StaticContainer.Container.Resolve<AdminChangeUserPasswordViewModel>();
        }
    }
}
