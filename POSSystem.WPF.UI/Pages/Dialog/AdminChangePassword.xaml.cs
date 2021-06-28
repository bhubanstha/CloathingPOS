using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.WPF.UI.Service;
using POSSystem.WPF.UI.ViewModel;

namespace POSSystem.WPF.UI.Views.Dialog
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
