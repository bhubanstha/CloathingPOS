using Autofac;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using System;
using System.Windows.Controls;
using System.Linq;

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
            ILogger logger = StaticContainer.Container.Resolve<ILogger>();
            model = new SalesViewModel(txtProductName, logger);
            model.CurrentProduct.SalesQuantity = 1;
            this.DataContext = model;            
        }

        private async void Txt_OnProductNameChange(object sender, TextChangedEventArgs e)
        {
            TextBox inp = sender as TextBox;
            //model.FilterProducts = model.Products.Where(p => p.Name.ToLower().StartsWith(inp.Text.ToLower())).ToList();
            //txtProduct.AutoCompleteItemSource = model.FilterProducts;
            model.FilterProducts = await model.GetFilteredProduct(inp.Text.ToLower());
            txtProductName.AutoCompleteItemSource = model.FilterProducts;
        }

        private void Txt_ProductSelected(object sender, EventArgs e)
        {
            if(txtProductName.SelectedItem != null)
            {
                var pr = txtProductName.SelectedItem as Inventory;
                model.CurrentProduct.ProductId = pr.Id;
                model.CurrentProduct.RetailRate = pr.RetailRate;
                model.CurrentProduct.PurchaseRate = pr.PurchaseRate;
                model.CurrentProduct.ProductName = pr.Name;
                model.CurrentProduct.Color = pr.Color;
                model.CurrentProduct.Size = pr.Size;
                model.CurrentProduct.CategoryId = pr.Category.Id;
                model.CurrentProduct.CategoryName = pr.Category.Name;
                model.CurrentProduct.BrandId = pr.Brand.Id;
                model.CurrentProduct.BrandName = pr.Brand.Name;
                model.CurrentProduct.ColorName = pr.ColorName;
                txtSalesQty.Maximum = (double)pr.Quantity;
                txtSalesQty.Minimum = NegativeItemInCart(pr.Id);
            }
        }


        private async void Txt_OnCustomerNameChange(object sender, TextChangedEventArgs e)
        {
            TextBox inp = sender as TextBox;
            string searchText = inp.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                model.FilterCustomers = await model.GetFilteredCustomer(searchText);
                txtCustomerName.AutoCompleteItemSource = model.FilterCustomers;
            }
            else
            {
                model.CurrentCustomer.Id = 0;
                model.CurrentCustomer.Name = string.Empty;
                model.CurrentCustomer.Address = string.Empty;
                model.CurrentCustomer.GoogleMap = string.Empty;
                model.CurrentCustomer.Mobile1 = string.Empty;
                model.CurrentCustomer.Mobile2 = string.Empty;
            }
        }

        private void Txt_CustomerSelected(object sender, EventArgs e)
        {
            if (txtCustomerName.SelectedItem != null)
            {
                var cust = txtCustomerName.SelectedItem as Customer;
                model.CurrentCustomer.Id = cust.Id;
                model.CurrentCustomer.Name = cust.Name;
                model.CurrentCustomer.Address = cust.Address;
                model.CurrentCustomer.GoogleMap = cust.GoogleMap;
                model.CurrentCustomer.Mobile1 = cust.Mobile1;
                model.CurrentCustomer.Mobile2 = cust.Mobile2;                
            }
        }


        private double NegativeItemInCart(Int64 productId)
        {
            if (model.CurrentCart != null && model.CurrentCart.Count > 0)
            {
                var itm = model.CurrentCart.Where(x => x.ProductId == productId).FirstOrDefault();
                if(itm != null)
                {
                    return itm.SalesQuantity * -1;
                }
            }
            return 1;
        }
    }
}
