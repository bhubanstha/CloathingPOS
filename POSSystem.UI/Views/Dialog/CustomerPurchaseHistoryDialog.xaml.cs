using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;

namespace POSSystem.UI.Views.Dialog
{
    /// <summary>
    /// Interaction logic for AddNewCustomerDialog.xaml
    /// </summary>
    public partial class CustomerPurchaseHistoryDialog : CustomDialog
    {
        public CustomerPurchaseHistoryDialog()
        {
            InitializeComponent();
            var model = StaticContainer.Container.Resolve<CustomerPurchaseHistoryViewModel>();
            this.DataContext = model;
        }
    }
}
