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
using POS.BusinessRule;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for SalesView.xaml
    /// </summary>
    public partial class SalesView : UserControl
    {
        private SalesViewModel model;
        private BillBO _billBo;
        public SalesView()
        {
            InitializeComponent();
            _billBo = new BillBO();
            model = new SalesViewModel();
            model.CurrentProduct = new Sales();
            model.CurrentProduct.SalesQuantity = 1;
            model.CurrentProduct.Bill = new Bill
            {
                BillNo = _billBo.GetNewBillNo(),
                BillDate = DateTime.Now
            };
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
                model.CurrentProduct.Rate = pr.RetailRate;
                txtRetailRate.Value = (double) model.CurrentProduct.Rate;
                //txtProduct.Text = pr.Name + " - " + pr.Size;
            }
        }
    }
}
