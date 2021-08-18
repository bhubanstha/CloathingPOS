using Autofac;
using log4net;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class AddCustomerViewModel : ViewModelBase
    {
        private ILog _logger;
        private IEventAggregator _eventAggregator;
        private CustomerWrapper _customer;
        private string _saveButtonText = "Save Customer";
        private bool _isUpdate = false;
        public string SaveButtonText
        {
            get { return _saveButtonText; }
            set { _saveButtonText = value; OnPropertyChanged(); }
        }

        public CustomerWrapper Customer
        {
            get { return _customer; }
            set { _customer = value; }
        }

        public ICommand SaveCustomerCommand { get; set; }
        public ICommand CloseDialogCommand { get; set; }

        public AddCustomerViewModel(ILogger logger, IEventAggregator eventAggregator)
        {
            _logger = logger.GetLogger(typeof(AddCustomerViewModel));
            _eventAggregator = eventAggregator;
            SaveButtonText = "Save Customer";
            Customer = new CustomerWrapper(new Customer());
            SaveCustomerCommand = new DelegateCommand(OnSaveCustomerExecute);
            CloseDialogCommand = new DelegateCommand(OnDialogCloseExecute);

            eventAggregator.GetEvent<UpdateCustomerInfoEvent>().Subscribe(OnCustomerEditReceive);
        }

        private void OnCustomerEditReceive(CustomerWrapper obj)
        {
            Customer.Id = obj.Id;
            Customer.Name = obj.Name;
            Customer.Address = obj.Address;
            Customer.GoogleMap = obj.GoogleMap;
            Customer.Mobile1 = obj.Mobile1;
            Customer.Mobile2 = obj.Mobile2;
            SaveButtonText = "Update Customer";
            _isUpdate = true;
        }

        private void OnDialogCloseExecute()
        {
            MetroWindow _window = StaticContainer.ThisApp.MainWindow as MetroWindow;
            AddNewCustomerDialog dialog = StaticContainer.Container.Resolve<AddNewCustomerDialog>();
            _window.HideMetroDialogAsync(dialog);
        }

        private async void OnSaveCustomerExecute()
        {
            try
            {
                CustomerBO bo = new CustomerBO();

                long i = Customer.Id > 0 ? await bo.Update(Customer.Model) : await bo.Save(Customer.Model) ;
                string msg = "";
                if (i > 0)
                {
                    msg = _isUpdate ? "Customer information updated" : "New customer added into system";
                    StaticContainer.ShowNotification("Customer", msg, NotificationType.Success);
                }
                else
                {
                    msg = _isUpdate ? "Customer information couldn't be updated" : "New customer couldn't be added into system";
                    StaticContainer.ShowNotification("Customer", msg, NotificationType.Warning);
                }
                Reset();
            }
            catch (Exception ex)
            {
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, NotificationType.Error);
                _logger.Error("OnSaveCustomerExecute", ex);
            }
            finally
            {
                OnDialogCloseExecute();
                SaveButtonText = "Save Customer";
            }
        }

        private void Reset()
        {
            Customer.Id = 0;
            Customer.Name = string.Empty;
            Customer.Address = string.Empty;
            Customer.GoogleMap = string.Empty;
            Customer.Mobile1 = string.Empty;
            Customer.Mobile2 = string.Empty;
        }
    }
}
