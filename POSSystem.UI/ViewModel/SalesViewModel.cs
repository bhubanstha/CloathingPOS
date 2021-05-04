using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesViewModel: ViewModelBase
    {
        private InventoryBO inventoryBO;
        private MetroWindow _window;
        private Bill _bill;
        private bool _generateBill = true;
        private ObservableCollection<Sales> _currentCart;
        private Sales _currentProduct;
        public List<Inventory> Products { get; set; }
        public List<Inventory> FilterProducts { get; set; }
        public CultureInfo CultureInfo { get; set; }

        public List<Inventory> Cart { get; set; }

        public Sales CurrentProduct { 
            get { return _currentProduct; }
            set { 
                _currentProduct = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Sales> CurrentCart {
            get { return _currentCart; }
            set
            {
                _currentCart = value;
                OnPropertyChanged();
            }
        
        }

        private SalesBO _salesBo;
        private BillBO _billBo;


        public ICommand AddItemToCartCommand { get; }
        public SalesViewModel()
        {
            _window = Application.Current.MainWindow as MetroWindow;
            CultureInfo = StaticContainer.CultureInfo;
            GetInventory();
            AddItemToCartCommand = new DelegateCommand<DataGrid>(AddItemToCart);
            //inventoryBO = new InventoryBO();
            //CurrentCart = new List<Sales>
            //{
            //    new Sales
            //    {
            //        Discount = 100,
            //        Rate= 2500,
            //        SalesQuantity = 1,
            //        ProductId = 3,
            //        Inventory = inventoryBO.GetById(3)
            //    },
            //    new Sales
            //    {
            //        Discount = 300,
            //        Rate= 1400,
            //        SalesQuantity = 1,
            //        ProductId = 2,
            //        Inventory = inventoryBO.GetById(2)
            //    }
            //};
        }

        private void AddItemToCart(DataGrid grid)
        {
            if (_generateBill)
            {
                _bill = new Bill
                {
                    BillDate = DateTime.Now,
                    VAT = 0
                };

                _billBo = new BillBO();
                _billBo.AddBillToMemory(ref _bill);
                CurrentCart = new ObservableCollection<Sales>();
                _generateBill = false;
            }
            _salesBo = new SalesBO();
            Sales p = new Sales
            {
                Bill = CurrentProduct.Bill,
                BillNo = CurrentProduct.BillNo,
                Discount = CurrentProduct.Discount,
                Id = CurrentProduct.Id,
                ProductId = CurrentProduct.ProductId,
                Inventory = CurrentProduct.Inventory,
                Rate = CurrentProduct.Rate,
                SalesQuantity = CurrentProduct.SalesQuantity
            };

            p = _salesBo.ManageCartItem(p, CurrentCart, ref _bill);
            CurrentCart.Add(p);
            ClearProduct();
            //grid.ItemsSource = CurrentCart;
            //grid.InvalidateVisual();
            // _window.ShowMessageAsync("title", "some message", MessageDialogStyle.Affirmative, StaticContainer.DialogSettings);
        }

        private void GetInventory()
        {
            inventoryBO = new InventoryBO();
            Products = inventoryBO.GetAll();
        }

        private void ClearProduct()
        {

            CurrentProduct.Bill = new Bill();
            CurrentProduct.BillNo = 0;
            CurrentProduct.Discount = 0;
            CurrentProduct.Id = 0;
            CurrentProduct.ProductId = 0;
            CurrentProduct.Inventory = new Inventory();
            CurrentProduct.Rate = 0;
            CurrentProduct.SalesQuantity = 1;
        }
       
    }
}
