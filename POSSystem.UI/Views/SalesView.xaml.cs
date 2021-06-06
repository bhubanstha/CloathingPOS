using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.ViewModel;
using System;
using System.Linq;
using System.Windows.Controls;

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
                Id = _billBo.GetNewBillNo(),
                BillDate = DateTime.Now
            };
            this.DataContext = model;
            OpenPdf();
            
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
                txtSalesQty.Maximum = (double)model.CurrentProduct.Inventory.Quantity;
                //txtProduct.Text = pr.Name + " - " + pr.Size;
            }
        }

        private void OpenPdf()
        {
            string pdfPath = @"D:\Download\pprotect.pdf";
            moonPdfPanel.OpenFile(pdfPath);
            moonPdfPanel.PageRowDisplay = MoonPdfLib.PageRowDisplayType.ContinuousPageRows;
        }

        private void PackIconBootstrapIcons_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PDFViewerWindow window = new PDFViewerWindow(@"D:\Download\pprotect.pdf");
            window.Show();
        }
    }
}
