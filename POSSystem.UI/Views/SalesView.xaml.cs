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
using POS.Model;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for SalesView.xaml
    /// </summary>
    public partial class SalesView : UserControl
    {
        private SalesViewModel model;
        public SalesView()
        {
            InitializeComponent();
            model = new SalesViewModel();
            model.CurrentProduct = new Sales();
            this.DataContext = model;
            
        }

        private void Txt_OnProductNameChange(object sender, TextChangedEventArgs e)
        {
            TextBox inp = sender as TextBox;
            model.FilterProducts = model.Products.Where(p => p.Name.ToLower().StartsWith(inp.Text.ToLower())).ToList();
            txtProduct.AutoCompleteItemSource = model.FilterProducts;
        }

        private void Txt_ProductSelected(object sender, EventArgs e)
        {
            if(txtProduct.SelectedItem != null)
            {
                var pr = txtProduct.SelectedItem as Inventory;                
                model.CurrentProduct.Inventory = pr;

                //txtProduct.Text = pr.Name + " - " + pr.Size;
            }
        }
    }
}
