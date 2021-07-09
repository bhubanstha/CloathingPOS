using Autofac;
using log4net;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Model.ViewModel;
using POS.Utilities.PDF;
using POSSystem.UI.Controls;
using POSSystem.UI.Event;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesViewModel : ViewModelBase
    {
        //private Fields
        private string _currentPdfFilePath = "";
        private ILog _log;
        private InventoryBO inventoryBO;
        private ObservableCollection<SalesWrapper> _currentCart;
        private SalesWrapper _currentProduct;
        private AutoCompleteTextBox _productName;
        private bool _isBillGenerating = false;


        //Public Properties
        public List<Inventory> FilterProducts { get; set; }
        public CultureInfo CultureInfo { get; set; }
        public SalesWrapper CurrentProduct
        {
            get { return _currentProduct; }
            set
            {
                _currentProduct = value;
                OnPropertyChanged();
            }
        }

        public bool IsBillGenerating
        {
            get { return _isBillGenerating; }
            set { _isBillGenerating = value; OnPropertyChanged(); }
        }

        public BillWrapper CurrentBill { get; set; }
        public ObservableCollection<SalesWrapper> CurrentCart
        {
            get { return _currentCart; }
            set
            {
                _currentCart = value;
            }

        }
        public string CurrentPdfFilePath
        {
            get { return _currentPdfFilePath; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _currentPdfFilePath = value;
                }
            }
        }


        //Delegate Commands
        public ICommand AddItemToCartCommand { get; }
        public ICommand DeleteCartItemCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand ResetCommand { get; }

        //Constructor
        public SalesViewModel(AutoCompleteTextBox productName, ILogger logger)
        {
            _productName = productName;
            _log = logger.GetLogger(typeof(SalesViewModel));
            CurrentProduct = new SalesWrapper(new SalesModel());

            CurrentCart = new ObservableCollection<SalesWrapper>();
            CurrentBill = new BillWrapper(new BillModel())
            {
                BranchId = StaticContainer.ActiveBranchId,
                UserId = _loggedInUser.Id,
                CalculateVAT = StaticContainer.Shop.CalculateVATOnSales
            };
            CultureInfo = StaticContainer.CultureInfo;


            AddItemToCartCommand = new DelegateCommand(AddItemToCart, CanAdditemToCartExecute);
            DeleteCartItemCommand = new DelegateCommand<SalesWrapper>(RemoveItemFromCart);
            CheckoutCommand = new DelegateCommand(OnSalesCheckout, CanSalesCheckoutExecute);
            ResetCommand = new DelegateCommand(OnResetExecute);

            CurrentProduct.PropertyChanged += CurrentProduct_PropertyChanged;
            CurrentBill.PropertyChanged += CurrentBill_PropertyChanged;
            CurrentCart.CollectionChanged += CurrentCart_CollectionChanged;

            IEventAggregator eventAggregator = StaticContainer.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<BranchSwitchedEvent>().Subscribe(ResetSales);

            CreateNewBill();
        }

        private void ResetSales(BranchWrapper obj)
        {
            OnResetExecute();
        }

        private void OnResetExecute()
        {
            CurrentBill.VAT = 0;
            CurrentBill.GrandTotal = 0;
            CurrentBill.BillTo = "";
            CurrentBill.BillingAddress = "";
            CurrentBill.BillingPAN = "";
            CurrentCart.Clear();
            CurrentProduct.ProductId = 0;
            CurrentProduct.ProductName = "";
            CurrentProduct.RetailRate = 0;
            CurrentProduct.SalesQuantity = 1;
            CurrentProduct.Size = "";
            CurrentProduct.Discount = 0;
            CurrentProduct.ColorName = "";
            CurrentProduct.Color = "";
            CurrentProduct.CategoryName = "";
            CurrentProduct.CategoryId = 0;
            CurrentProduct.BrandName = "";
            CurrentProduct.BrandId = 0;
            _productName.Text = "";
            _productName.SelectedItem = null;
        }

        #region Property Events
        private void CurrentCart_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ((DelegateCommand)CheckoutCommand).RaiseCanExecuteChanged();
        }

        private void CurrentBill_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BillTo")
            {
                ((DelegateCommand)CheckoutCommand).RaiseCanExecuteChanged();
            };
        }
        private void CurrentProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ProductName" || e.PropertyName == "SalesQuantity")
            {
                ((DelegateCommand)AddItemToCartCommand).RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Can Execute Methods
        private bool CanSalesCheckoutExecute()
        {
            return CurrentCart.Count > 0 && !string.IsNullOrEmpty(CurrentBill.BillTo);
        }

        public bool CanAdditemToCartExecute()
        {
            return CurrentProduct != null && !string.IsNullOrEmpty(CurrentProduct.ProductName) && CurrentProduct.SalesQuantity != 0;
        }
        #endregion


        #region Command Execute Methods
        private void AddItemToCart()
        {
            try
            {

                ManageItemInCart();
                ClearProduct();
                StaticContainer.ShowNotification("Item Added", "Item added into cart for checkout.", NotificationType.Information);
            }
            catch (Exception ex)
            {
                _log.Error("AddItemToCart", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private void RemoveItemFromCart(SalesWrapper obj)
        {
            try
            {

                RemoveItemFromCurrentCart(obj.Model);
                CalculateVAT();
                StaticContainer.ShowNotification("Item Removed", "Selected item removed from the cart.", NotificationType.Error);
            }
            catch (Exception ex)
            {
                _log.Error("RemoveItemFromCart", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private async void OnSalesCheckout()
        {
            try
            {
                IsBillGenerating = true;
                await CheckoutCart();

                StaticContainer.ShowNotification("Checkout", "Cart checkout. Items sold successfully", NotificationType.Success);
            }
            catch (Exception ex)
            {
                _log.Error("OnSalesCheckout", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            finally
            {
                if (StaticContainer.Shop.PrintInvoice)
                {
                    string pdfPath = await CreatePdfFromCurrentCartItem();
                    IsBillGenerating = false;
                    PDFViewerWindow pDFViewer = new PDFViewerWindow(pdfPath, StaticContainer.Shop.PdfPassword);
                    pDFViewer.ShowDialog();

                }
                IsBillGenerating = false;
                ClearProduct(true);
            }
        }

        #endregion

        #region Private Methods
        private void CreateNewBill()
        {
            BillBO billBO = new BillBO();
            CurrentBill.BillNo = billBO.GetNewBillNo();
        }

        private void ClearProduct(bool clearAfterCheckout = false)
        {
            CurrentProduct.RetailRate = 0;
            CurrentProduct.Discount = 0;
            CurrentProduct.SalesQuantity = 1;
            CurrentProduct.ProductId = -1;
            CurrentProduct.Size = "";
            CurrentProduct.Color = "";
            CurrentProduct.CategoryName = "";
            CurrentProduct.CategoryId = -1;
            CurrentProduct.ProductName = "";
            _productName.SelectedItem = null;
            if (clearAfterCheckout)
            {
                CurrentBill.BillNo = CurrentBill.BillNo + 1;
                CurrentBill.BillingAddress = "";
                CurrentBill.BillingPAN = "";
                CurrentBill.BillTo = "";
                CurrentBill.VAT = 0;
                CurrentBill.GrandTotal = 0;

                CurrentCart.Clear();

            }
        }

        private void CalculateVAT()
        {

            decimal total = 0;
            decimal totalDiscount = 0;
            foreach (var item in CurrentCart)
            {
                decimal itemTotal = (item.RetailRate * item.SalesQuantity);
                decimal discount = item.Discount;
                total += itemTotal;
                totalDiscount += discount;
            }

            decimal vatAmount = StaticContainer.Shop.CalculateVATOnSales == true ? Math.Ceiling((13 * total) / 100) : 0;
            CurrentBill.VAT = vatAmount;
            CurrentBill.GrandTotal = total + vatAmount - totalDiscount;
        }

        private void ManageItemInCart()
        {
            try
            {
                bool result = UpdateItemInCurrentCart(CurrentProduct);
                if (!result)
                {
                    SalesWrapper saleitem = new SalesWrapper(new SalesModel());
                    saleitem.Copy(CurrentProduct);
                    CurrentCart.Add(saleitem);
                }
                CalculateVAT();
            }
            catch (Exception ex)
            {
                _log.Error("ManageItemInCart", ex);
            }
        }

        private Task CheckoutCart()
        {
            Task t = Task.Run(async () =>
            {
                try
                {

                    BillBO billBO = new BillBO();

                    Bill b = new Bill
                    {
                        BillDate = DateTime.Now,
                        BillingAddress = CurrentBill.BillingAddress,
                        BillingPAN = CurrentBill.BillingPAN,
                        BillTo = CurrentBill.BillTo,
                        VAT = CurrentBill.VAT,
                        BranchId = StaticContainer.ActiveBranchId,
                        CalculateVAT = StaticContainer.Shop.CalculateVATOnSales,
                        UserId = _loggedInUser.Id
                    };
                    Int64 billNo = billBO.CreateNewBill(ref b);
                    foreach (var item in CurrentCart)
                    {
                        SalesBO salesBO = new SalesBO();
                        Sales s = new Sales
                        {
                            SalesQuantity = item.SalesQuantity,
                            Rate = item.RetailRate,
                            Discount = item.Discount,
                            ProductId = item.ProductId,
                            BillNo = billNo
                        };
                        await salesBO.CheckoutSales(s);
                    }
                }
                catch
                {
                    throw;
                }
            });

            return t;
        }

        private bool UpdateItemInCurrentCart(SalesWrapper obj)
        {
            try
            {
                var item = CurrentCart.Where(x => x.ProductId == obj.ProductId).FirstOrDefault();
                if (item != null)
                {
                    item.SalesQuantity += obj.SalesQuantity;
                    item.Discount = obj.Discount;
                    if (item.SalesQuantity == 0)
                    {
                        CurrentCart.Remove(item);
                    }
                    return true;
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _log.Error("UpdateItemInCurrentCart", ex);
                return false;
            }
        }

        private void RemoveItemFromCurrentCart(SalesModel obj)
        {
            var item = CurrentCart.Where(x => x.ProductId == obj.ProductId).FirstOrDefault();
            if (item != null)
            {
                CurrentCart.Remove(item);
            }
        }

        #endregion

        #region Public Methods
        public async Task<string> CreatePdfFromCurrentCartItem()
        {
            CurrentPdfFilePath = await new CreatePDF()
                .CreateInvoice(CurrentBill.Model, CurrentCart.Select(x => x.Model).ToList(), StaticContainer.Shop);
            return CurrentPdfFilePath;
        }

        public List<Inventory> GetFilteredProduct(string productName)
        {
            inventoryBO = new InventoryBO();
            List<Inventory> filteredInventory = inventoryBO.GetAllActiveProducts(productName, StaticContainer.ActiveBranchId);
            return filteredInventory;
        }

        #endregion
    }
}
