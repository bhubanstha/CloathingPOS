using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Wrapper;
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
    public class CustomerListViewModel : ViewModelBase
    {
        private ObservableCollection<CustomerWrapper> customers;
        private string _customerFilter = string.Empty;
        private bool _isDataLoading = false;
        private IEventAggregator _eventAggregator;
        public ICollectionView CustomerCollectionView { get; private set; }

        public ObservableCollection<CustomerWrapper> CustomerList
        {
            get { return customers; }
            set { customers = value; OnPropertyChanged(); }
        }

        public string CustomerFilter
        {
            get { return _customerFilter; }
            set
            {
                _customerFilter = value;
                OnPropertyChanged();
                if (CustomerCollectionView != null)
                {
                    CustomerCollectionView.Refresh();
                }
            }
        }
        

        public bool IsDataLoading
        {
            get { return _isDataLoading; }
            set { _isDataLoading = value; OnPropertyChanged(); }
        }


        public ICommand LoadCustomerDataCommand { get; private set; }
        public ICommand EditCustomerCommand { get; private set; }
        public ICommand ShowPurchaseHistoryCommand { get; private set; }

        public CustomerListViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            LoadCustomerDataCommand = new DelegateCommand(OnLoadCustomerExecute);
            EditCustomerCommand = new DelegateCommand<CustomerWrapper>(OnCustomerEditExecute);
            ShowPurchaseHistoryCommand = new DelegateCommand<CustomerWrapper>(OnPurchaseHistoryExecute);
        }

        private void OnPurchaseHistoryExecute(CustomerWrapper obj)
        {
            MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            CustomerPurchaseHistoryDialog dialog = StaticContainer.Container.Resolve<CustomerPurchaseHistoryDialog>();
            _eventAggregator.GetEvent<ShowCustomerPurchaseEvent>().Publish(obj.Id);
            _window.ShowMetroDialogAsync(dialog);
        }

        private void OnCustomerEditExecute(CustomerWrapper obj)
        {
            MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            AddNewCustomerDialog dialog = StaticContainer.Container.Resolve<AddNewCustomerDialog>();
            _eventAggregator.GetEvent<UpdateCustomerInfoEvent>().Publish(obj);
            _window.ShowMetroDialogAsync(dialog);
        }

        private async void OnLoadCustomerExecute()
        {
            IsDataLoading = true;
            CustomerBO bo = new CustomerBO();
            List<Customer> custList = await bo.GetAll();
            long i = 1;
            CustomerList = new ObservableCollection<CustomerWrapper>();
            foreach (Customer customer in custList)
            {
                CustomerWrapper cw = new CustomerWrapper(customer);
                cw.SN = i;
                CustomerList.Add(cw);
                i++;
            }
            CustomerCollectionView = CollectionViewSource.GetDefaultView(CustomerList);
            CustomerCollectionView.Filter = FilterCustomer;
            IsDataLoading = false;
        }


        private bool FilterCustomer(object obj)
        {
            if (obj is CustomerWrapper c)
            {
                return c.Name.StartsWith(CustomerFilter, StringComparison.InvariantCultureIgnoreCase) ||
                    c.Mobile1.StartsWith(CustomerFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
