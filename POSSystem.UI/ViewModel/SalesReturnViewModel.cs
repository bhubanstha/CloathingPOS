using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.ValueBoxes;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesReturnViewModel : NotifyPropertyChanged
    {

        private Int64 _billNo;
        private ObservableCollection<Sales> _sales;
        private Bill _bill;
        private MetroWindow _window;

        public Int64 BillNo
        {
            get { return _billNo; }
            set { _billNo = value; OnPropertyChanged(); }
        }


        public ObservableCollection<Sales> Sales
        {
            get { return _sales; }
            set { _sales = value; OnPropertyChanged(); }
        }

        public Bill Bill
        {

            get { return _bill; }
            set { _bill = value; OnPropertyChanged(); }

        }

        public ICommand SearchCommand { get; }
        public ICommand ReturnItemCommand { get; }

        public SalesReturnViewModel()
        {
            _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            SearchCommand = new DelegateCommand(OnSearchExecute, OnSearchCanExecute);
            ReturnItemCommand = new DelegateCommand<Sales>(OnSalesReturn);
            Sales = new ObservableCollection<Sales>();
        }

        private async void OnSalesReturn(Sales item)
        {
            try
            {
                _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
                int salesReturn = item.SalesQuantity;
                if (item.SalesQuantity > 1)
                {
                    salesReturn++;
                    while (salesReturn > item.SalesQuantity)
                    {
                        var returnVal = await _window.ShowInputAsync("Sales Return", $"Enter sales return quantity (Max: {item.SalesQuantity} )", StaticContainer.DialogSettings);
                        int.TryParse(returnVal, out salesReturn);
                    }
                }

                if(salesReturn>0)
                {
                    SalesBO salesBO = new SalesBO();                   
                    int i = 0;
                    if (salesReturn == item.SalesQuantity)
                    {
                        i = await salesBO.Remove(item);
                    }
                    else
                    {
                        item.SalesQuantity -= salesReturn;
                        i = await salesBO.Update(item); 
                    }
                    InventoryBO inventoryBO = new InventoryBO();
                    await inventoryBO.Restock(item.Inventory, salesReturn);
                    LoadSales();
                    StaticContainer.NotificationManager.Show(new NotificationContent
                    {
                        Title = "Sales Return",
                        Message = $"Item: {item.Inventory.Name} , Return Qty: {salesReturn} returned on {DateTime.Now.ToString("yyyy/MM/dd")}",
                        Type = NotificationType.Success
                    });
                }                
            }
            catch (Exception ex)
            {
                StaticContainer.NotificationManager.Show(new NotificationContent
                {
                    Title = "Error",
                    Message = ex.Message,
                    Type = NotificationType.Error
                });
            }
        }

        private void OnSearchExecute()
        {
            LoadSales();
            if(Sales.Count==0)
            {
                Flyout f = StaticContainer.NoSearchResultFlyout;
                f.IsOpen = !f.IsOpen;
            }
        }

        private async void LoadSales()
        {
            try
            {
                SalesBO salesBO = new SalesBO();
                List<Sales> _sales = await salesBO.GetSalesByBillNo(BillNo);
                Sales.Clear();
                foreach (Sales item in _sales)
                {
                    Bill = item.Bill;
                    Sales.Add(item); 
                }
            }
            catch (Exception ex)
            {
                StaticContainer.NotificationManager.Show(new NotificationContent
                {
                    Title = "Error",
                    Message = ex.Message,
                    Type = NotificationType.Error
                });
            }
        }

        private bool OnSearchCanExecute()
        {
            return true;
        }

    }


}
