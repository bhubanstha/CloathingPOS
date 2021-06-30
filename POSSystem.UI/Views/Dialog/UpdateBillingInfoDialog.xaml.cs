using Autofac;
using Autofac.Core;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace POSSystem.UI.Views.Dialog
{
    /// <summary>
    /// Interaction logic for UpdateInventoryDialog.xaml
    /// </summary>
    public partial class UpdateBillingInfoDialog : CustomDialog
    {
        private BillingViewModel model;
        public UpdateBillingInfoDialog()
        {
            InitializeComponent();

            try
            {
                model = StaticContainer.Container.Resolve<BillingViewModel>(); // (new List<Parameter> { new TypedParameter(this.GetType(), this) });
                model.Dialog = this;
                this.DataContext = model;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Loaded += UpdateInventoryDialog_Loaded;
        }

        private void UpdateInventoryDialog_Loaded(object sender, RoutedEventArgs e)
        {
            txtQuantity.SelectAll();
        }

        protected override void OnShown()
        {
            base.OnShown();
            this.Invoke(() => this.MinHeight = 0);
        }
    }
}
