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
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class SalesReturnViewModel : NotifyPropertyChanged
    {

        private Int64 _billNo;
        private ObservableCollection<Sales> _sales;
        private Bill _bill;

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

        public SalesReturnViewModel()
        {
            SearchCommand = new DelegateCommand(OnSearchExecute, OnSearchCanExecute);
            Sales = new ObservableCollection<Sales>();
        }

        private void OnSearchExecute()
        {
            LoadSales();
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
                    Sales.Add(item); Sales.Add(item);
                    Sales.Add(item); Sales.Add(item);
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
