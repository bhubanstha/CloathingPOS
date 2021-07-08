using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities;
using POS.Utilities.PDF;
using POSSystem.UI.Event;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Views.Dialog;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesListViewModel : ViewModelBase
    {
        private ICollection<Sales> _salesList;
        private DateTime? _billingDate = DateTime.Today;
        private IEventAggregator _eventAggregator;
        private ILog _log;
        private UpdateBillingInfoDialog _billingDialog;
        private bool _isBillingInfoUpdated = false;
        private bool _isBillGenerating = false;

        public ICollection<Sales> SalesList
        {
            get { return _salesList; }
            set
            {
                _salesList = value;
                OnPropertyChanged();
            }
        
        }

       

        public bool IsBillGenerating
        {
            get { return _isBillGenerating ; }
            set { _isBillGenerating  = value; OnPropertyChanged(); }
        }



        public DateTime? BillingDate
        {
            get { return _billingDate; }
            set { _billingDate = value; OnPropertyChanged(); }
        }


        public ICollectionView SalesCollectionView { get; private set; }
        public ICommand SearchBillCommand { get; private set; }
        public ICommand EditBillingInfoCommand { get; private set; }
        public ICommand ReprintBillCommand { get; private set; }
       

        public SalesListViewModel(IEventAggregator eventAggregator, ILogger logger, UpdateBillingInfoDialog dialog)
        {
            _eventAggregator = eventAggregator;
            _log = logger.GetLogger(typeof(SalesListViewModel));
            _billingDialog = dialog;
            SearchBillCommand = new DelegateCommand(OnBillSearchExecute);
            EditBillingInfoCommand = new DelegateCommand<Int64?>(OnBillInfoEditExeucte);
            ReprintBillCommand = new DelegateCommand<Int64?>(OnBillReprintExeucte);
            eventAggregator.GetEvent<BillingInfoUpdateEvent>().Subscribe(OnBillInfoUpdateReceive);
        }

        private async void OnBillReprintExeucte(long? obj)
        {
            try
            {
                string pdfPath = FileUtility.GetInvoicePdfPath(obj.Value);
                IsBillGenerating = true;
                if (string.IsNullOrEmpty(pdfPath))
                {
                    StaticContainer.ShowNotification("Error", $"The invoice file is in use by another application. Close the invoice and retry", NotificationType.Information);
                }
                else
                {
                    //Create Bill if existing billing file not found or billing info is changed
                    if (_isBillingInfoUpdated || !FileUtility.CheckInvoiceFileExists(pdfPath))
                    {
                        List<Sales> salesRecord = SalesList.Where(x => x.BillNo == obj.Value).ToList();
                        pdfPath = await new CreatePDF().CreateInvoice(salesRecord[0].Bill, salesRecord, StaticContainer.Shop);
                        
                    }
                    //OpenBill
                    PDFViewerWindow window = new PDFViewerWindow(pdfPath, StaticContainer.Shop.PdfPassword);
                    IsBillGenerating = false;
                    window.ShowDialog();
                }
               
            }
            catch (Exception ex)
            {
                _log.Error("OnBillReprintExeucte", ex);
            }
            finally
            {
                IsBillGenerating = false;
            }
        }

        private void OnBillInfoUpdateReceive(BillingInfoUpdateEventArgs obj)
        {
            if (obj.Action == EventAction.Update)
            {
                _isBillingInfoUpdated = true;
                LoadSales();
            }
        }

        private void OnBillInfoEditExeucte(long? obj)
        {
            MetroWindow window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            BillingInfoUpdateEventArgs args = new BillingInfoUpdateEventArgs
            {
                BillId = obj.Value,
                Action = EventAction.Edit
            };
            _eventAggregator.GetEvent<BillingInfoUpdateEvent>().Publish(args);
            window.ShowMetroDialogAsync(_billingDialog);
        }

        private void OnBillSearchExecute()
        {
            LoadSales();
        }

        void LoadSales()
        {
            if (BillingDate.HasValue)
            {
                SalesBO bo = new SalesBO();
                var billList = bo.GetAllOnDate(BillingDate.Value, StaticContainer.ActiveBranchId);
                SalesList = new ObservableCollection<Sales>(billList);
                if(SalesList.Count == 0)
                {
                    Flyout f = StaticContainer.NoSearchResultFlyout;
                    f.IsOpen = !f.IsOpen;
                }
               
            }
            else
            {
                SalesList = new ObservableCollection<Sales>();
            }
            SalesCollectionView = CollectionViewSource.GetDefaultView(SalesList);
            SalesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Sales.BillNo)));
        }
    }
}
