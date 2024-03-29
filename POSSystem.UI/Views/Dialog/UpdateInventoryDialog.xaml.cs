﻿using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System.Windows;

namespace POSSystem.UI.Views.Dialog
{
    /// <summary>
    /// Interaction logic for UpdateInventoryDialog.xaml
    /// </summary>
    public partial class UpdateInventoryDialog : CustomDialog
    {
        private InventoryHistoryViewModel model;
        public UpdateInventoryDialog()
        {
            InitializeComponent();

            try
            {
                model = StaticContainer.Container.Resolve<InventoryHistoryViewModel>();
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
    }
}
