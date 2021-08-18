using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;

namespace POSSystem.UI.Views.Dialog
{
    /// <summary>
    /// Interaction logic for AddNewCustomerDialog.xaml
    /// </summary>
    public partial class AddNewCustomerDialog : CustomDialog
    {
        public AddNewCustomerDialog()
        {
            InitializeComponent();
            var model = StaticContainer.Container.Resolve<AddCustomerViewModel>();
            this.DataContext = model;
        }
    }
}
