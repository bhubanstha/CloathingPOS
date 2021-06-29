using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.UIModel;
using Prism.Commands;
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
        private DateTime? _billingDate;
        public ICollection<Sales> SalesList
        {
            get { return _salesList; }
            set
            {
                _salesList = value;
                OnPropertyChanged();
            }
        
        }
       

        public DateTime? BillingDate
        {
            get { return _billingDate; }
            set { _billingDate = value; OnPropertyChanged(); }
        }


        public ICollectionView SalesCollectionView { get; private set; }


        public ICommand SearchBillCommand { get; private set; }

        public SalesListViewModel()
        {
            SearchBillCommand = new DelegateCommand(OnBillSearchExecute);
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
                var billList = bo.GetAllOnDate(BillingDate.Value);
                SalesList = new ObservableCollection<Sales>(billList);
               
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
