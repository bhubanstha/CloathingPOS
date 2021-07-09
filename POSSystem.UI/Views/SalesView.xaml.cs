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
            model.FilterProducts = model.GetFilteredProduct(inp.Text.ToLower());
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
