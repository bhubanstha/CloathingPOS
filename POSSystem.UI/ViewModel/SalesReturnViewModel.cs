﻿using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Model.ViewModel;
using POS.Utilities.PDF;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesReturnViewModel : ViewModelBase
    {
        private ILog _log;
        private Int64 _billNo = 1;
        private ObservableCollection<Sales> _sales;
        private Bill _bill;
        private MetroWindow _window;
        private bool _isBillGenerating = false;

        public bool IsBillGenerating
        {
            get { return _isBillGenerating; }
            set { _isBillGenerating = value; }
        }


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
        public ICommand PrintReceiptCommand { get; }

        public SalesReturnViewModel(ILogger logger)
        {
            _log = logger.GetLogger(typeof(SalesReturnViewModel));
            _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            SearchCommand = new DelegateCommand(OnSearchExecute, OnSearchCanExecute);
            ReturnItemCommand = new DelegateCommand<Sales>(OnSalesReturn);
            PrintReceiptCommand = new DelegateCommand(OnReprintReceipt);
            Sales = new ObservableCollection<Sales>();
        }

        private async void OnReprintReceipt()
        {
            if (Sales.Count > 0)
            {
                IsBillGenerating = true;
                string path = await new CreatePDF()
                    .CreateInvoice(Sales[0].Bill, Sales.ToList(), StaticContainer.Shop);
                PDFViewerWindow window = new PDFViewerWindow(path, StaticContainer.Shop.PdfPassword);
                IsBillGenerating = false;
                window.ShowDialog();
            }
            else
            {
                StaticContainer.ShowNotification("Print", "No billable items found to create bill", NotificationType.Information);
            }
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
                        Sales.Remove(item);
                    }
                    else
                    {
                        item.SalesQuantity -= salesReturn;
                        i = await salesBO.Update(item);
                        var itm = Sales.Where(x => x.Id == item.Id).FirstOrDefault();
                        itm.SalesQuantity = item.SalesQuantity;
                    }
                    //Restock Item into inventory
                    InventoryBO inventoryBO = new InventoryBO();
                    await inventoryBO.Restock(item.Inventory, salesReturn);

                    //Remove BillingInfo if no sales record exists for current billno
                    ManageBills(item.Bill, Sales.ToList());
                    //await LoadSales();
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
                _log.Error("OnSalesReturn", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
        }

        private async void OnSearchExecute()
        {
            await LoadSales();
            if(Sales.Count==0)
            {
                Flyout f = StaticContainer.NoSearchResultFlyout;
                f.IsOpen = !f.IsOpen;
            }
        }

        private void  ManageBills(Bill bill, List<Sales> salesRecord)
        {
           Task t =  Task.Run(async () =>
            {
                BillBO billBO = new BillBO();
                int totalRemainingSales = salesRecord.Count;// billBO.GetRemainingSalesCount(bill.Id);
                if(totalRemainingSales == 0)
                {
                    await billBO.Remove(BillNo);
                }
                else if(bill.CalculateVAT)
                {
                    decimal vatAmount = await RecalculateVAT( salesRecord);
                    bill.VAT = vatAmount;
                    await billBO.Update(bill);
                }
            });

        }


        private Task<decimal> RecalculateVAT(List<Sales> salesRecord)
        {
            Task<decimal> t = Task.Run(() => {
                SalesBO salesBO = new SalesBO();
                //List<Sales> sales = await salesBO.GetSalesByBillNo(billNo, StaticContainer.ActiveBranchId);
                decimal total = 0;
                foreach (Sales item in salesRecord)
                {
                    decimal itemTotal = (item.SalesRate * item.SalesQuantity);
                    total += itemTotal;
                }
                decimal vat = (13 * total) / 100;
                return vat;
            });
            return t;
        }
        private async Task<int> LoadSales()
        {
            try
            {
                SalesBO salesBO = new SalesBO();
                List<Sales> _sales = await salesBO.GetSalesByBillNo(BillNo, StaticContainer.ActiveBranchId);
                Sales.Clear();
                foreach (Sales item in _sales)
                {
                    Bill = item.Bill;
                    Sales.Add(item); 
                }
            }
            catch (Exception ex)
            {
                _log.Error("LoadSales", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
            }
            return 1;
        }

        private bool OnSearchCanExecute()
        {
            return true;
        }

    }


}
