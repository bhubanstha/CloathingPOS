using Autofac;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.Views.Dialog;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class CustomerPurchaseHistoryViewModel : ViewModelBase
    {
        private ILog _logger;
        private IEventAggregator _eventAggregator;
        private ObservableCollection<Sales> _salesList;

        public ICollectionView SalesCollectionView { get; private set; }
        public ObservableCollection<Sales> SalesList
        {
            get { return _salesList; }
            set
            {
                _salesList = value;
                OnPropertyChanged();
            }

        }

        public ICommand CloseDialogCommand { get; set; }

        public CustomerPurchaseHistoryViewModel(ILogger logger, IEventAggregator eventAggregator)
        {
            _logger = logger.GetLogger(typeof(AddCustomerViewModel));
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<ShowCustomerPurchaseEvent>().Subscribe(OnDialogOpenAsync);
            CloseDialogCommand = new DelegateCommand(OnDialogCloseExecute);
        }


        private async void OnDialogOpenAsync(long customerId)
        {
            List<Sales> sales = await LoadSales(customerId);
            if (sales == null || sales.Count == 0)
            {
                SalesList = null;
                SalesCollectionView = null;
                Flyout f = StaticContainer.NoSearchResultFlyout;
                f.IsOpen = !f.IsOpen;
            }
            else
            {
                MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
                CustomerPurchaseHistoryDialog dialog = StaticContainer.Container.Resolve<CustomerPurchaseHistoryDialog>();
                SalesList = new ObservableCollection<Sales>(sales);
                SalesCollectionView = CollectionViewSource.GetDefaultView(SalesList);
                SalesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Sales.BranchName)));
                SalesCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Sales.BillNo)));
                await _window.ShowMetroDialogAsync(dialog);
            }
        }

        private void OnDialogCloseExecute()
        {
            try
            {
                MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
                CustomerPurchaseHistoryDialog dialog = StaticContainer.Container.Resolve<CustomerPurchaseHistoryDialog>();
                _window.HideMetroDialogAsync(dialog);
            }
            catch (Exception ex)
            {
                _logger.Error("CustomerPurchaseHistoryViewModel.OnDialogCloseExecute()", ex);
            }
        }

        Task<List<Sales>> LoadSales(long customerId)
        {
            return Task.Run(async () =>
            {


                SalesBO bo = new SalesBO();
                List<Sales> billList = await bo.GetCustomerPurchaseHitory(customerId);
                if (billList.Count > 0)
                {
                    return billList;
                }
                return null;

            });
        }
    }
}
